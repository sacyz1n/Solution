using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public partial interface IGameDbService
    {
    }

    public partial class GameDbService 
        : Repository.Contexts.DbServiceBase<Repository.Contexts.GameDbContext>
        , IGameDbService
    {
        public GameDbService(ILogger<GameDbService> logger)
            : base(Log.LogManager.LoggerFactory, 
                  Repository.Contexts.ConnectionString.GetConnectionString(
                      Repository.Contexts.Constants.GameDB))
        {
        }
    }
}
