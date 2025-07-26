namespace GameService.Services
{
    public static class MemoryDbKey
    {
        public static string UserAuthKey = "USER_AUTH_{0}";

        public static string UserRequestLock = "USER_REQUEST_LOCK_{0}";

        public static string MakeUserAuthKey(long accountNo) 
            => string.Format(UserAuthKey, accountNo);

        public static string MakeUserRequestLockKey(long accountNo) 
            => string.Format(UserRequestLock, accountNo);
    }

    public static class MemoryDbExpireTime
    {
        public const int UserAuthMin = 60; // 60분

        public const int UserRequestLockSeconds = 3;
    }
}
