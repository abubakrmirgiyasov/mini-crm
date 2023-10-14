using Microsoft.EntityFrameworkCore;
using MiniCrm.UI.Models;
using Task = MiniCrm.UI.Models.Task;

namespace MiniCrm.UI.Services;

public class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<EmployeeProject> EmployeeProjects { get; set; }

    public DbSet<Manager> Managers { get; set; }

    public DbSet<Task> Tasks { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Employee>()
            .HasIndex(x => x.Email)
            .IsUnique(true);

        builder.Entity<EmployeeProject>()
            .HasKey(x => new
            {
                x.ProjectId,
                x.EmployeeId,
            });

        builder.Entity<Task>()
            .HasOne(x => x.Executor)
            .WithMany(x => x.Executors)
            .HasForeignKey(x => x.ExecutorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Task>()
            .HasOne(x => x.Author)
            .WithMany(x => x.Authors)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
