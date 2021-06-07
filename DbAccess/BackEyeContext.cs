using Microsoft.EntityFrameworkCore;
using Model;
using System;

namespace DbAccess
{
    public class BackEyeContext : DbContext
    {
        public BackEyeContext(DbContextOptions<BackEyeContext> options)
     : base(options)
        {
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public DbSet<Persons> Persons { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Photos> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persons>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(
                "server=localhost;user=root;database=BackEyeDb;password=123456;");
        }
    }
}
