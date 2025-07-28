using MemoryPack;

namespace Client.Shared
{
    public static class MediaType
    {
        public const E_SupportMediaType UseMediaType = E_SupportMediaType.MemoryPack;

        public enum E_SupportMediaType
        {
            MemoryPack = 0,
            Json = 1,
        }
    }

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
        public string Token { get; set; } = string.Empty;
        public string MemberId { get; set; } = string.Empty;

        public E_PlatformType PlatformType { get; set; } = E_PlatformType.None;
    }

    [MemoryPackable]
    public partial class LoginResponse : BaseResponse
    {
        public long AccountNo { get; set; } = 0;
        public string MemberId { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
    }
}
