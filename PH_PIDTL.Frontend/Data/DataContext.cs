using Microsoft.EntityFrameworkCore;
using PH_PIDTL;

namespace PH_PIDTL.Frontend.Data
{
    public class DataContext : DbContext

    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<PH_PIDTL> PH_PIDTL { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PH_PIDTL>()
                .ToTable("label_data")
                .HasKey(p => p.id);

        }
    }
}
