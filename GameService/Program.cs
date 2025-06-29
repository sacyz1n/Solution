using GameService.Formatters;
using GameService.Services;
using Log;
using Server.Shared.Services;
using Utf8StringInterpolation;
using ZLogger;
using ZLogger.Providers;

namespace GameService
{
    public class ConnectionStrings
    {
        public string Redis { get; set; } = string.Empty;

        public string GameDB { get; set; } = string.Empty;

        public string GlobalDB { get; set; } = string.Empty;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
            {
                Args = args,
                ApplicationName = "GameService",
                ContentRootPath = baseDirectory,
            });

            if (builder.Configuration["Environment"] == null)
            {
                Console.WriteLine("Not found Environment. Use --environment");
                return;
            }

            Environment.SetEnvironmentVariable("Env", builder.Environment.EnvironmentName);

            Console.WriteLine($"OS Version : {System.Environment.OSVersion}");
            Console.WriteLine($"UserName:{System.Environment.UserName}");
            Console.WriteLine($"Environment:{Environment.GetEnvironmentVariable("Env")}");

            var configuration = builder.Configuration;
            LoadConnectionStrings(configuration);

            builder.Services.AddTransient<IGameDbService, GameDbService>();
            builder.Services.AddTransient<IGlobalDbService, GlobalDbService>();
            builder.Services.AddSingleton<IFileResourceService, FileResourceService>();
            builder.Services.AddSingleton<ISessionService, SessionService>();
            builder.Services.AddControllers(options =>
            {
                options.OutputFormatters.Insert(0, new MemoryPackOutputFormatter());
                options.InputFormatters.Insert(0, new MemoryPackInputFormatter());
            });
            builder.Services.AddControllers();

            builder.Logging.ClearProviders();
            builder.Logging.AddZLoggerConsole(options =>
                options.UsePlainTextFormatter(formatter =>
                {
                    formatter.SetPrefixFormatter($"{0}|{1:short}|", (in MessageTemplate template, in LogInfo info) => template.Format(info.Timestamp, info.LogLevel));
                    formatter.SetSuffixFormatter($" ({0})", (in MessageTemplate template, in LogInfo info) => template.Format(info.Category));
                    formatter.SetExceptionFormatter((writer, ex) => Utf8String.Format(writer, $"{ex.Message}"));
                })
            );
            builder.Logging.AddZLoggerRollingFile(configure =>
            {
                configure.FilePathSelector = (now, index) => $"{baseDirectory}/logs/{now:yyyyMMdd}_{index}.log";
                configure.RollingInterval = RollingInterval.Day;
                configure.RollingSizeKB = 1024 * 64; // 64MB
                configure.UsePlainTextFormatter(formatter =>
                {
                    formatter.SetPrefixFormatter($"{0}|{1:short}|", (in MessageTemplate template, in LogInfo info) => template.Format(info.Timestamp, info.LogLevel));
                    formatter.SetSuffixFormatter($" ({0})", (in MessageTemplate template, in LogInfo info) => template.Format(info.Category));
                    formatter.SetExceptionFormatter((writer, ex) => Utf8String.Format(writer, $"{ex.Message}"));
                });
            });
            builder.Logging.AddNotifyLogger(new TelegramNotification());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Global logger 설정
            var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
            LogManager.SetLoggerFactory(loggerFactory, "Global");

            // 서버 데이터 로드
            var serverDataPath = Path.Combine(Environment.CurrentDirectory, builder.Configuration["ServerDataPath"]!);
            app.Services.GetService<IFileResourceService>()!.LoadTableData(serverDataPath);

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        internal static void LoadConnectionStrings(IConfiguration configuration)
        {
            var connectionStrings = new ConnectionStrings();
            configuration.GetSection(nameof(ConnectionStrings)).Bind(connectionStrings);

            if (string.IsNullOrEmpty(connectionStrings.Redis))
                throw new ArgumentException("Redis connection string is not set.");

            if (string.IsNullOrEmpty(connectionStrings.GameDB))
                throw new ArgumentException("GameDB connection string is not set.");

            if (string.IsNullOrEmpty(connectionStrings.GlobalDB))
                throw new ArgumentException("GlobalDB connection string is not set.");

            Repository.Contexts.ConnectionString.s_ReloadConnectionString = 
                () => LoadConnectionStrings(configuration);

            Repository.Contexts.ConnectionString.Load(Repository.Contexts.Constants.GameDB, connectionStrings.GameDB);
            Repository.Contexts.ConnectionString.Load(Repository.Contexts.Constants.GlobalDB, connectionStrings.GlobalDB);
        }
    }
}
