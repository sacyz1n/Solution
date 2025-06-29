using Microsoft.EntityFrameworkCore;

namespace Repository.Contexts
{
    public class GameDbContext : BaseDbContext
    {
        public GameDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
