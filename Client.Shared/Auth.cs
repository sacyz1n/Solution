using MemoryPack;

namespace Client.Shared
{
    public enum E_PlatformType
    {
        None = 0,
        GOOGLE = 1,
        APPLE = 2,
        ONESTORE = 3,

        DEV = 99,
    }

    [MemoryPackable]
    public partial class LoginRequest
    {
        public string MemberId { get; set; } = string.Empty;
    }

    [MemoryPackable]
    public partial class LoginResponse : BaseResponse
    {
        public long AccountNo { get; set; } = 0;
        public string MemberId { get; set; } = string.Empty;
    }
}
