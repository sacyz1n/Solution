using GameService.Services;
using Server.Shared.Models.Auth;

namespace GameService.Logic.Login
{
    public static class DefaultSetting
    {
        public static async ValueTask<bool> Execute(long accountNo, int gameDBIndex, IGameDbService gameDbService)
        {
            // 캐릭터 정보 생성
            await gameDbService.InsertCharacterInfo(gameDBIndex, new Repository.GameDB.CharacterInfo
            {
                AccountNo = accountNo,
                Level = 1,
                Exp = 0,
                StageLevel = 1,
                AttackLevel = 1,
                HPLevel = 1,
                DefenseLevel = 1,
            });



            return true;
        }
    }
}
