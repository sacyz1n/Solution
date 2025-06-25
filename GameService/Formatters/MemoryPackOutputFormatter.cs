using MemoryPack;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace GameService.Formatters
{
    public class MemoryPackOutputFormatter : OutputFormatter
    {
        // Serialize 가능 여부 체크 함수
        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            var accept = context.HttpContext.Request.Headers["Accept"].ToString();

            if (string.IsNullOrEmpty(accept))
                return false;

            if (!accept.Contains(Constants.MemoryPackContentType))
                return false;

            return true;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = Constants.MemoryPackContentType;

            if (context.Object == null)
            {
                response.StatusCode = 204; // No Content
                return;
            }

            try
            {
                var data = MemoryPackSerializer.Serialize(context.ObjectType!, context.Object);
                await response.Body.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500; // Internal Server Error
                await response.WriteAsync($"Error serializing object: {ex.Message}");
            }
        }
    }
}
