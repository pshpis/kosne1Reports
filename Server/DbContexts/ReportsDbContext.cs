using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Server.DbContexts
{
    public sealed class ReportsDbContext : DbContext
    {
        public ReportsDbContext(DbContextOptions<ReportsDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<TaskChangeModel> Changes { get; set; }
        public DbSet<TaskCommentModel> Comments { get; set; }
        public DbSet<ReportModel> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeModel>().HasOne(e => e.Boss);
            modelBuilder.Entity<TaskModel>().HasOne(t => t.Executor);
            modelBuilder.Entity<TaskChangeModel>().HasOne(change => change.Task);
            modelBuilder.Entity<TaskChangeModel>().HasOne(change => change.Employee);
            modelBuilder.Entity<TaskCommentModel>().HasOne(comment => comment.ChangeInfo);
            modelBuilder.Entity<ReportModel>().HasOne(report => report.Employee);
            modelBuilder.Entity<ReportModel>().HasMany(report => report.Tasks).WithOne();

            base.OnModelCreating(modelBuilder);
        }
    }
}