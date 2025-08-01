using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Repository.Contexts
{
    public class DbContextInfo<TDbContext> where TDbContext : BaseDbContext
    {
        private TDbContext _dbContext = null;
        private SqlKata.Compilers.MySqlCompiler _sqlKataCompiler = null;
        private SqlKata.Execution.QueryFactory _queryFactory = null;

        public SqlKata.Execution.QueryFactory QueryFactory
        {
            get
            {
                if (_queryFactory == null)
                    _queryFactory = new SqlKata.Execution.QueryFactory(_dbContext.Database.GetDbConnection(), _sqlKataCompiler);
                return _queryFactory;
            }
        }

        public DbContextInfo(TDbContext dbContext, SqlKata.Compilers.MySqlCompiler compiler, SqlKata.Execution.QueryFactory queryFactory)
        {
            this._dbContext = dbContext;
            this._sqlKataCompiler = compiler;
            this._queryFactory = queryFactory;
        }
    }

    public interface IQueryFactoryProvider
    {
    }

    public abstract class DbServiceBase : IQueryFactoryProvider
    {
        private readonly ILoggerFactory _loggerFactory;

        protected DbServiceBase(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(_loggerFactory));
        }

        protected DbContextInfo<TDbContext> CreateDbContext<TDbContext>(string dbName) where TDbContext : BaseDbContext
        {
            if (string.IsNullOrWhiteSpace(dbName))
                return null;

            var connectionString = ConnectionString.GetConnectionString(dbName);
            if (string.IsNullOrWhiteSpace(connectionString))
                return null;

            var options = new DbContextOptionsBuilder<TDbContext>()
                    .UseMySql(connectionString, ConnectionString.s_ServerVersion, builder =>
                    {
                        // Collection 을 파라미터로 사용 가능하도록 설정
                        builder.TranslateParameterizedCollectionsToConstants();
                        builder.EnablePrimitiveCollectionsSupport();
                    })
                    .UseLoggerFactory(_loggerFactory)
                    .Options;

            var dbContext = (Activator.CreateInstance(typeof(TDbContext), new object[] { options }) as TDbContext)!;
            var sqlKataCompiler = new SqlKata.Compilers.MySqlCompiler();
            var queryFactory = new SqlKata.Execution.QueryFactory(dbContext.Database.GetDbConnection(), sqlKataCompiler);
            return new(dbContext, sqlKataCompiler, queryFactory);
        }
    }
}
