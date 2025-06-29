using Repository;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public partial interface IGlobalDbService
    {
        // Create
        public Task<bool> InsertAccountInfo(AccountInfo accountInfo);
    }

    public partial class GlobalDbService
    {
        public async Task<bool> InsertAccountInfo(AccountInfo accountInfo)
        {
            var result = await QueryFactory.Query(GlobalDB<AccountInfo>.Get())
                                           .InsertAsync(accountInfo);

            if (result != 1)
                return false;

            return true;
        }
    }
}
