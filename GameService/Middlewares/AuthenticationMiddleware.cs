using Client.Shared;
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

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        //public static async Task<AuthCheck> ReadMemoryPackFromBody(HttpRequest request)
        //{
        //    using var ms = new MemoryStream();
        //    await request.Body.CopyToAsync(ms);

        //    ReadOnlySpan<byte> span = ms.ToArray();

        //    // MemoryPack Union Tag는 첫 바이트에 존재
        //    byte tag = span[0];

        //    if (!_unionTypeMap.TryGetValue(tag, out var actualType))
        //    {
        //        throw new InvalidOperationException($"Unknown MemoryPack Union tag: {tag}");
        //    }

        //    object? obj = MemoryPackSerializer.Deserialize(actualType, span);

        //    return (obj as AuthCheck)!;
        //}

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
                Log.LogManager.Logger.LogError("AuthenticationMiddleware: AuthCheck is null");
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

            // 토큰 검증


            await this._next(context);
        }
    }
}
