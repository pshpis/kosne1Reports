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

        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<TaskChangeModel> Changes { get; set; }
        public DbSet<TaskCommentModel> Comments { get; set; }
        public DbSet<ReportModel> Reports { get; set; }
        public DbSet<SprintReportModel> SprintReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeModel>().HasOne(e => e.Boss);
            modelBuilder.Entity<TaskModel>().HasOne(t => t.Executor);
            modelBuilder.Entity<TaskChangeModel>().HasOne(change => change.Task);
            modelBuilder.Entity<TaskChangeModel>().HasOne(change => change.Employee);
            modelBuilder.Entity<TaskCommentModel>().HasOne(comment => comment.ChangeInfo);
            modelBuilder.Entity<ReportModel>().HasOne(report => report.Employee);
            modelBuilder.Entity<ReportModel>().HasMany(report => report.Tasks).WithOne();
            modelBuilder.Entity<SprintReportModel>().HasOne(spr => spr.Employee);
            modelBuilder.Entity<SprintReportModel>().HasMany(spr => spr.Reports).WithOne();

            base.OnModelCreating(modelBuilder);
        }
    }
}