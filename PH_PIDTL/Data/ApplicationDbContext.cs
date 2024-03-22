using Microsoft.EntityFrameworkCore;
using Polynic.Models;

namespace Polynic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<PH_PIDTL> PH_PIDTLs { get; set; }

    }   
}
