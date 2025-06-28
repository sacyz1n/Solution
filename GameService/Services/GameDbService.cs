using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public interface IGameDbService
    {
    }

    public class GameDbService 
        : Repository.Contexts.DbServiceBase<Repository.Contexts.GameDbContext>
        , IGameDbService
    {
        public GameDbService(ILogger<GameDbService> logger, IConfiguration configuration)
            : base(Log.LogManager.LoggerFactory, "")
        {
        }
    }
}
