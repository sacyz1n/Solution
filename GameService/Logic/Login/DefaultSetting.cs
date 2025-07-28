using GameService.Services;

namespace GameService.Logic.Login
{
    public static class DefaultSetting
    {
        public static void Execute(long accountNo, int gameDbIndex, IGameDbService gameDbService)
        {
            // 캐릭터 정보 생성
            var characterInfo = new Repository.GameDB.CharacterInfo
            {
                AccountNo = 1,
                Level = 1,
                Exp = 0,
            };



        }
    }
}
