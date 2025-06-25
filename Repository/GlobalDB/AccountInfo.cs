using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.GlobalDB
{
    [PrimaryKey(nameof(AccountNo))]
    [Table("account_info")]
    public partial class AccountInfo
    {
        public long AccountNo { get; set; } = 0;

        public string Id { get; set; } = string.Empty;
    }
}
