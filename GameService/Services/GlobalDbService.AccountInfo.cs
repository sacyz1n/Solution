using Repository;
using Repository.GlobalDB;
using SqlKata.Execution;

namespace GameService.Services
{
    public partial interface IGlobalDbService
    {
        public Task<long> InsertAccountInfo(AccountInfo accountInfo);
        public Task<AccountInfo> GetAccountInfo(string memberId);
    }

    public partial class GlobalDbService
    {
        public async Task<long> InsertAccountInfo(AccountInfo accountInfo)
        {
            var accountNo = await QueryFactory().Query(GlobalDB<AccountInfo>.Get())
                                                .InsertGetIdAsync<long>(accountInfo);


            return accountNo;
        }

        public async Task<AccountInfo> GetAccountInfo(string memberId)
        {
            var result = await QueryFactory().Query(GlobalDB<AccountInfo>.Get())
                                             .Where(nameof(AccountInfo.MemberId), memberId)
                                             .FirstOrDefaultAsync<AccountInfo>();

            return result;
        }
    }
}
