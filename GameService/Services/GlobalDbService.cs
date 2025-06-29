using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public interface IGlobalDbService
    {
        public Task<bool> InsertAccountInfo(AccountInfo accountInfo);

        public Task<AccountInfo> GetAccountInfo(string memberId);
    }

    public class GlobalDbService
        : Repository.Contexts.DbServiceBase<Repository.Contexts.GlobalDbContext>
        , IGlobalDbService
    {
        public GlobalDbService(ILogger<GlobalDbService> logger, IOptions<ConnectionStrings> options)
            : base(Log.LogManager.LoggerFactory, Repository.Contexts.ConnectionString.GetConnectionString(
                Repository.Contexts.Constants.GlobalDB))
        {
        }

        public async Task<bool> InsertAccountInfo(AccountInfo accountInfo)
        {
            var result = await QueryFactory.Query(GlobalDB<AccountInfo>.Get())
                                           .InsertAsync(accountInfo);

            if (result != 1)
                return false;

            return true;
        }

        public async Task<AccountInfo> GetAccountInfo(string memberId)
        {
            var result = await QueryFactory.Query(GlobalDB<AccountInfo>.Get())
                                           .Where(nameof(AccountInfo.MemeberId), memberId)
                                           .FirstOrDefaultAsync<AccountInfo>();

            return result;
        }

    }
}
