using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Client.Shared
{
    public abstract class BaseResponse
    {
        public int ErrorCode { get; set; } = ErrorCodes.SUCCESS;

        public string ErrorDesc { get; set; } = string.Empty;

        internal void SetErrorCodeAndDesc(int errorCode, string errorDesc)
        {
            this.ErrorCode = errorCode;
            this.ErrorDesc = errorDesc;
        }
    }


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

    public static class ErrorCodes
    {
        public const int SUCCESS = 0;

        public const int LOGIN_ERROR = 1;
    }
}
