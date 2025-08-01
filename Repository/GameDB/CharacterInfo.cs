using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.GameDB
{
    [PrimaryKey(nameof(AccountNo))]
    [Table("character_info")]
    public partial class CharacterInfo
    {
        public long AccountNo { get; set; } = 0;

        [Required]
        public int Level { get; set; }

        [Required]
        public long Exp { get; set; }

        [Required]
        public int StageLevel { get; set; }

        [Required]
        public int AttackLevel { get; set; }

        [Required]
        public int HPLevel { get; set; }

        [Required]
        public int DefenseLevel { get; set; }

    }
}
