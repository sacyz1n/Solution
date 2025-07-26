namespace Client.Shared
{
    public static class ErrorCodes
    {
        public const int SUCCESS = 0;

        public const int LOGIN_ERROR = 1;

        public const int USER_NOT_AUTHORIZED = 100;

        public const int INVALID_TOKEN = 101;

        public const int REDIS_EXCEPTION = 900;
    }
}
