using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Client.Shared
{
    public static class ErrorCodeExtensions
    {
        public static T SetErrorCode<T>(this T output, int errorCode, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0) where T : BaseResponse
        {
            var logger = Log.LogManager.Logger;

            output.SetErrorCodeAndDesc(
                errorCode: errorCode,
                errorDesc: string.Format("{0:X}.{1}", memberName, sourceLineNumber));

            if (errorCode != 0 && logger != null)
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation($"{typeof(T).Name} ErrorCode:{errorCode} {memberName}.{sourceLineNumber}");
            }

            return output;
        }
    }
}
