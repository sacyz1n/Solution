using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;
using Repository.Contexts;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public partial class GlobalDbService : DbServiceBase, IGlobalDbService
    {

        private DbContextInfo<GlobalDbContext> _dbContext = null!;

        public QueryFactory QueryFactory() => _dbContext.QueryFactory;

        public GlobalDbService(ILogger<GlobalDbService> logger, IOptions<ConnectionStrings> options)
            : base(Log.LogManager.LoggerFactory)
        {
            this._dbContext = base.CreateDbContext<Repository.Contexts.GlobalDbContext>(Repository.Contexts.Constants.GetGlobalDB());
        }
    }
}
