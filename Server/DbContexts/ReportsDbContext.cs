using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.DbContexts
{
    public sealed class ReportsDbContext : DbContext
    {
        public ReportsDbContext(DbContextOptions<ReportsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>();
            base.OnModelCreating(modelBuilder);
        }
    }
}