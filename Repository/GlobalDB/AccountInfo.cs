using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.GlobalDB
{
    [PrimaryKey(nameof(AccountNo))]
    [Table("account_info")]
    public partial class AccountInfo
    {
        public long AccountNo { get; set; } = 0;

        public string MemberId { get; set; } = string.Empty;

        public byte PlatformType { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime CreateTime { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime LoginTime { get; set; }
    }
}
