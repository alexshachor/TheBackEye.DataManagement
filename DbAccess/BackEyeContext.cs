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
        public DbSet<Person> Persons { get; set; }
        //public DbSet<Photo> Photos { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<StudentLesson> StudentLessons { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Person>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
