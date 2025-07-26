using MemoryPack;

namespace Client.Shared
{

    public abstract partial class BaseResponse
    {
        public int ErrorCode { get; set; } = ErrorCodes.SUCCESS;

        public string ErrorDesc { get; set; } = string.Empty;

        internal void SetErrorCodeAndDesc(int errorCode, string errorDesc)
        {
            this.ErrorCode = errorCode;
            this.ErrorDesc = errorDesc;
        }
    }
}
