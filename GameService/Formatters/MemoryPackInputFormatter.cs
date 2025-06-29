using MemoryPack;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace GameService.Formatters
{
    public class MemoryPackInputFormatter : InputFormatter
    {
        public MemoryPackInputFormatter()
        {
            SupportedMediaTypes.Add(Constants.MemoryPackContentType);
        }

        // Deserialize 가능 여부 체크 함수
        public override bool CanRead(InputFormatterContext context)
        {
            var contentType = context.HttpContext.Request.ContentType;
            if (string.IsNullOrEmpty(contentType))
                return false;

            if (!contentType.Contains(Constants.MemoryPackContentType))
                return false;

            return true;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var modelType = context.ModelType;

            try
            {
                using (var reader = new MemoryStream())
                {
                    await context.HttpContext.Request.Body.CopyToAsync(reader);

                    var model = MemoryPackSerializer.Deserialize(modelType, reader.ToArray());
                    if (model == null)
                        return await InputFormatterResult.FailureAsync();

                    return await InputFormatterResult.SuccessAsync(model);
                }
            }
            catch (Exception ex)
            {
                Log.LogManager.Logger.LogError(ex, "MemoryPackInputFormatter ReadRequestBodyAsync Error");
                return await InputFormatterResult.FailureAsync();
            }
        }
    }
}
