﻿using Microsoft.EntityFrameworkCore;
using Polynic.Models;

namespace Polynic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<PH_PIDTL> PH_PIDTL { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PH_PIDTL>()
                .ToTable("PH_PIDTL")
                .HasNoKey();

        }
    }
}
