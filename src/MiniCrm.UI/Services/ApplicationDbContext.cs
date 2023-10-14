using Microsoft.EntityFrameworkCore;
using MiniCrm.UI.Models;
using System.Data;

namespace MiniCrm.UI.Services;

public class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<EmployeeProject> EmployeeProjects { get; set; }

    public DbSet<Manager> Managers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<EmployeeProject>()
            .HasKey(x => new
            {
                x.ProjectId,
                x.EmployeeId,
            });
    }
}
