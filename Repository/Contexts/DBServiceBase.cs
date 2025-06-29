using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Repository.Contexts
{
    public abstract class DbServiceBase<TDbContext> where TDbContext : BaseDbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        private string _connectionString = string.Empty;

        private TDbContext _dbContext = null;

        private SqlKata.Compilers.MySqlCompiler _sqlKataCompiler = null;

        private SqlKata.Execution.QueryFactory _queryFactory = null;

        protected SqlKata.Execution.QueryFactory QueryFactory
        {
            get
            {
                if (_queryFactory != null)
                    return _queryFactory;

                var dbContext = GetDbContext();
                if (dbContext == null)
                    throw new InvalidOperationException("Database context is not initialized.");

                _sqlKataCompiler = new SqlKata.Compilers.MySqlCompiler();
                _queryFactory = new SqlKata.Execution.QueryFactory(dbContext.Database.GetDbConnection(), _sqlKataCompiler);
                return _queryFactory;
            }
        }

        protected DbServiceBase(ILoggerFactory loggerFactory, string connectionString)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(_loggerFactory));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected TDbContext GetDbContext()
        {
            if (_dbContext != null)
                return _dbContext;

            if (string.IsNullOrWhiteSpace(_connectionString))
                return null;

            var options = new DbContextOptionsBuilder<TDbContext>()
                    .UseMySql(_connectionString, ConnectionString.s_ServerVersion, builder =>
                    {
                        // Collection 을 파라미터로 사용 가능하도록 설정
                        builder.TranslateParameterizedCollectionsToConstants();
                        builder.EnablePrimitiveCollectionsSupport();
                    })
                    .UseLoggerFactory(_loggerFactory)
                    .Options;

            _dbContext = (Activator.CreateInstance(typeof(TDbContext), new object[] { options }) as TDbContext)!;
            return _dbContext;
        }
    }
}
