using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public interface IGameDbService
    {
        public Task GetAccountInfo(long accountNo);
    }

    public class GameDbService 
        : Repository.Contexts.DbServiceBase<Repository.Contexts.GameDbContext>
        , IGameDbService
    {
        public GameDbService(ILogger<GameDbService> logger, IConfiguration configuration)
            : base(Log.LogManager.LoggerFactory, "")
        {
        }

        public async Task GetAccountInfo(long accountNo)
        {
            var result = await QueryFactory.Query(GlobalDB<AccountInfo>.Get())
                                           .Where(nameof(AccountInfo.Id), accountNo)
                                           .FirstOrDefaultAsync<AccountInfo>();

            if (result == null)
            {

            }
        }

    }
}
