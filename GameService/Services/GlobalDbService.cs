using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public partial class GlobalDbService
        : Repository.Contexts.DbServiceBase<Repository.Contexts.GlobalDbContext>
        , IGlobalDbService
    {
        public GlobalDbService(ILogger<GlobalDbService> logger, IOptions<ConnectionStrings> options)
            : base(Log.LogManager.LoggerFactory, Repository.Contexts.ConnectionString.GetConnectionString(
                Repository.Contexts.Constants.GlobalDB))
        { }
    }
}
