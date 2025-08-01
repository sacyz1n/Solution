using Repository;
using Repository.GameDB;
using Server.Shared.Models.Auth;
using SqlKata.Execution;
using System.IO.Hashing;

namespace GameService.Services
{
    public partial interface  IGameDbService
    {
        public Task<bool> InsertCharacterInfo(int gameDbIndex, CharacterInfo characterInfo);

        public Task<CharacterInfo> GetCharacterInfo(int gameDbIndex, long accountNo);
    }

    public partial class GameDbService
    {
        public async Task<bool> InsertCharacterInfo(int gameDbIndex, CharacterInfo characterInfo)
        {
            var affectedCnt = await QueryFactory(gameDbIndex)
                                        .Query(GameDB<CharacterInfo>.Get())
                                        .InsertAsync(characterInfo);

            if (affectedCnt != 1)
                return false;

            return true;
        }

        public async Task<CharacterInfo> GetCharacterInfo(int gameDbIndex, long accountNo)
        {
            var characterInfo = await QueryFactory(gameDbIndex).Query(GameDB<CharacterInfo>.Get())
                                            .Where(nameof(CharacterInfo.AccountNo), accountNo)
                                            .FirstOrDefaultAsync();

            return characterInfo;
        }
    }
}
