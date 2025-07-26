using Microsoft.EntityFrameworkCore;
using Repository.GlobalDB;

namespace Repository.Contexts
{
    public partial class GlobalDbContext : BaseDbContext
    {
        public GlobalDbContext() { }

        public GlobalDbContext(DbContextOptions<GlobalDbContext> options)
            : base(options)
        {
        }

        public DbSet<AccountInfo> account_info { get; set; } = null!;
    }
}
