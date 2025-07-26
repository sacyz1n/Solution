using Repository;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public partial interface IGlobalDbService
    {
        // Create
        public Task<long> InsertAccountInfo(AccountInfo accountInfo);
    }

    public partial class GlobalDbService
    {
        public async Task<long> InsertAccountInfo(AccountInfo accountInfo)
        {
            var accountNo = await QueryFactory.Query(GlobalDB<AccountInfo>.Get())
                                           .InsertGetIdAsync<long>(accountInfo);


            return accountNo;
        }
    }
}
