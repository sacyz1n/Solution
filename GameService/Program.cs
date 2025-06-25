using GameService.Formatters;
using GameService.Services;
using Log;
using Server.Shared.Services;
using ZLogger;
using ZLogger.Providers;

namespace GameService
{
    public class ConnectionStrings
    {
        public string Redis { get; private set; } = string.Empty;

        public string GameDB { get; private set; } = string.Empty;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
            {
                Args = args,
                ApplicationName = "GameService",
                ContentRootPath = AppDomain.CurrentDomain.BaseDirectory,
            });

            Console.WriteLine($"OS Version : {System.Environment.OSVersion}");
            Console.WriteLine($"Environment:{System.Environment.UserName}");

            var configuration = builder.Configuration;
            builder.Services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)));

            builder.Services.AddSingleton<IFileResourceService, FileResourceService>();
            builder.Services.AddSingleton<IDbAccessService, DbAccessService>();
            builder.Services.AddSingleton<ISessionService, SessionService>();
            builder.Services.AddControllers(options =>
            {
                options.OutputFormatters.Insert(0, new MemoryPackOutputFormatter());
                options.InputFormatters.Insert(0, new MemoryPackInputFormatter());
            });
            builder.Services.AddControllers();

            builder.Logging.ClearProviders();
            builder.Logging.AddNotifyLogger(new TelegramNotification());
            builder.Logging.AddZLoggerRollingFile(configure =>
            {
                configure.FilePathSelector = (now, index) => $"{Environment.CurrentDirectory}/logs/{now:yyyyMMdd}_{index}.log";
                configure.RollingInterval = RollingInterval.Day;
                configure.RollingSizeKB = 1024 * 1024; // 1GB
                configure.UseFormatter(() => new Log.ZLoggerFormatter());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // ���� ������ �ε�
            app.Services.GetService<IFileResourceService>()!.LoadTableData();

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
