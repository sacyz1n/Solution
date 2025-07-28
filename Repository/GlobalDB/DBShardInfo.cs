using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.GlobalDB
{
    [PrimaryKey(nameof(DBIndex), nameof(DBType))]
    [Table("db_shard_info")]
    public partial class DBShardInfo
    {
        [Required]
        public int DBIndex { get; set; }

        [Required]
        public byte DBType { get; set; }

        [Required]
        public string DBHost { get; set; } = string.Empty;

        [Required]
        public int DBPort { get; set; }
    }
}
