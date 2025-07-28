using Microsoft.EntityFrameworkCore;

namespace Repository.Contexts
{
    public class GameDbContext : BaseDbContext
    {
        public GameDbContext() { }

        public GameDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<GameDB.CharacterInfo> character_info { get; set; } = null!;
    }
}
