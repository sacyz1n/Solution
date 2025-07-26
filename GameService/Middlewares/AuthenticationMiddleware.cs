namespace GameService.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next = null;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var formString = context.Request.Path.Value;

            // 로그인 요청은 제외
            if (string.Compare(formString, "/", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(formString, "/login", StringComparison.OrdinalIgnoreCase) == 0)

            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
                return;
            }

            // 로그인 요청이 아닌 경우, Request Body를 EnableBuffering하여 다시 읽을 수 있도록 설정
            context.Request.EnableBuffering();

            using (var streamReader = new StreamReader(context.Request.Body, System.Text.Encoding.UTF8, true, 4096, true))
            {
                // Request Body를 읽어 로그에 기록
                var requestBody = await streamReader.ReadToEndAsync();
                Log.LogManager.Logger.LogInformation($"Request Body: {requestBody}");
            }


            // Position을 0으로 초기화하여 다음 미들웨어에서 읽을 수 있도록 함
            context.Request.Body.Position = 0;

            await this._next(context);
        }
    }
}
