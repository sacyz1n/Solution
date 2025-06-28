using Client.Shared;
using Server.Shared.Models.Platform.Strategies;
using System.Collections.Frozen;

namespace Server.Shared.Models.Platform
{
    public static class StrategyStatic
    {
        private static FrozenDictionary<E_PlatformType, IUserPlatform> s_userPlatforms = new
            Dictionary<E_PlatformType, IUserPlatform>()
        {
            [E_PlatformType.DEV] = new DevPlatform(),
            [E_PlatformType.GOOGLE] = new GooglePlatform(),
        }.ToFrozenDictionary();

        public static IUserPlatform GetStrategy(E_PlatformType platformType)
        {
            if (!s_userPlatforms.TryGetValue(platformType, out var strategy))
                return null;

            return strategy;
        }
    }
}
