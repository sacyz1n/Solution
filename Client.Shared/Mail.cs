using MemoryPack;

namespace Client.Shared
{
    [MemoryPackable]
    public partial class MailInfo
    {
        public long MailNo { get; set; } = 0;

        public string Title { get; set; } = string.Empty;

        public string Msg { get; set; } = string.Empty;
    }

    [MemoryPackable]
    public partial class MailListRequest
    {
        public long LastMailNo { get; set; } = 0;
    }

    [MemoryPackable]
    public partial class MailListResponse : BaseResponse
    {
        public MailInfo[] Infos { get; set; } = null;
    }

}
