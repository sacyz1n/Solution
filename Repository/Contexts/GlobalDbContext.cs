using Microsoft.EntityFrameworkCore;
using Repository.GlobalDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contexts
{
    public class GlobalDbContext : DbContext
    {
        public DbSet<AccountInfo> account_info { get; set; } = null!;
    }
}
