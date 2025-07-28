using Client.Shared;

namespace GameService.Logic.Login
{
    public static class FirebaseFacade
    {
        public static bool Initialize()
        {
            return true;
        }

        public static async Task<int> VerifyToken(string token, string memberId)
        {
            try
            {
                // 토큰 검증 시도
                var verifiedToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                if (verifiedToken == null)
                {
                    return ErrorCodes.FIREBASE_INVALID_TOKEN;
                }

                if (string.IsNullOrEmpty(verifiedToken.Uid) == true)
                {
                    return ErrorCodes.FIREBASE_INVALID_UID;
                }

                // Uid 체크
                if (verifiedToken.Uid != memberId)
                {
                    return ErrorCodes.FIREBASE_INVALID_UID;
                }

                return ErrorCodes.SUCCESS;
            }
            catch (Exception)
            {
                return ErrorCodes.FIREBASE_VERIFY_TOKEN_ERROR;
            }
        }
    }
}
