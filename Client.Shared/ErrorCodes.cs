namespace Client.Shared
{
    public static class ErrorCodes
    {
        public const int SUCCESS = 0;

        public const int LOGIN_ERROR = 1;

        public const int LOGIN_ALREADY = 2;

        public const int INVALID_PARAM = 3;

        public const int INVALID_PLATFORM_TYPE = 4;

        public const int USER_NOT_AUTHORIZED = 100;

        public const int INVALID_TOKEN = 101;

        public const int FIREBASE_INVALID_TOKEN = 800;
        public const int FIREBASE_INVALID_UID = 801;
        public const int FIREBASE_VERIFY_TOKEN_ERROR = 810;

        public const int REDIS_EXCEPTION = 900;

        public const int SERVER_ERROR = 1000;
    }
}
