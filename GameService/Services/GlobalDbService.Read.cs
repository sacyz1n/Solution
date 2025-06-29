using Repository;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public partial interface IGlobalDbService
    {
        public Task<AccountInfo> GetAccountInfo(string memberId);
    }


    public partial class GlobalDbService
    {
        public async Task<AccountInfo> GetAccountInfo(string memberId)
        {
            var result = await QueryFactory.Query(GlobalDB<AccountInfo>.Get())
                                           .Where(nameof(AccountInfo.MemeberId), memberId)
                                           .FirstOrDefaultAsync<AccountInfo>();

            return result;
        }
    }
}
