using Client.Shared;
using GameService.Services;
using MemoryPack;

namespace GameService.Middlewares
{
    public class Authorization
    {
        public long AccountNo { get; set; } = 0;
        public string AuthToken { get; set; } = string.Empty;
    }

    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next = null;

        private readonly IMemoryDbService _memoryDbService = null;

        public AuthenticationMiddleware(RequestDelegate next, IMemoryDbService memoryDbService)
        {
            this._next = next;
            this._memoryDbService = memoryDbService;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var formString = context.Request.Path.Value;

            // 로그인 요청은 제외
            if (string.Compare(formString, "/", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(formString, "/Login", StringComparison.OrdinalIgnoreCase) == 0)

            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("Authorization", out var header))
            {
                Log.LogManager.Logger.LogError("AuthenticationMiddleware: Invalid Authorization");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var authHeaders = header.ToString().Split(',');
            if (authHeaders == null || authHeaders.Length != 2)
            {
                Log.LogManager.Logger.LogError("AuthenticationMiddleware: Invalid Authorization header format");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            if (long.TryParse(authHeaders[0], out var accountNo) == false)
            {
                Log.LogManager.Logger.LogError("AuthenticationMiddleware: Invalid AccountNo in Authorization header");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var token = authHeaders[1];
            if (string.IsNullOrEmpty(token) == true)
            {
                Log.LogManager.Logger.LogError("AuthenticationMiddleware: Invalid AuthToken in Authorization header");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var (isAuthorized, result) = await _memoryDbService.IsAuthorizedUser(accountNo);
            if (isAuthorized != ErrorCodes.SUCCESS)
            {
                Log.LogManager.Logger.LogError($"AuthenticationMiddleware: Unauthorized access for AccountNo: {accountNo}");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (await _memoryDbService.VerifyToken(result, token) == false)
            {
                Log.LogManager.Logger.LogError($"AuthenticationMiddleware: invalid token for AccountNo: {accountNo}");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // Redis Lock 
            if (await _memoryDbService.TryLockUserRequest(accountNo) == false)
            {
                Log.LogManager.Logger.LogError($"AuthenticationMiddleware: User request lock failed for AccountNo: {accountNo}");
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }

            await this._next(context);

            await _memoryDbService.UnlockUserRequest(accountNo);
        }
    }
}
