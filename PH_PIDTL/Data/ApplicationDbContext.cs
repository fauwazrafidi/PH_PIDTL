using Microsoft.EntityFrameworkCore;
using SHARED;


namespace Polynic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<PH_PIDTL> PH_PIDTL { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PH_PIDTL>()
                .ToTable("label_data")
                .HasKey(p => p.id);

        }
    }
}
