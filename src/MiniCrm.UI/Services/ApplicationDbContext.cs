using Microsoft.EntityFrameworkCore;
using MiniCrm.UI.Common;
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

    public DbSet<Role> Roles { get; set; }

    public DbSet<EmployeeRole> EmployeeRoles { get; set; }

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

        builder.Entity<EmployeeRole>()
            .HasKey(x => new
            {
                x.RoleId,
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

        var roles = new Role[]
        {
            new Role()
            {
                Id = Guid.NewGuid(),
                Name = Constants.ROLES[0],
                NormalizedName = "руководитель",
            },
            new Role()
            {
                Id = Guid.NewGuid(),
                Name = Constants.ROLES[1],
                NormalizedName = "менеджер проекта",
            },
            new Role()
            {
                Id = Guid.NewGuid(),
                Name = Constants.ROLES[2],
                NormalizedName = "сотрудник",
            }
        };

        var salt = Hasher.GetSalt();

        var employees = new Employee[]
        {
            new Employee()
            {
                Id = Guid.NewGuid(),
                FirstName = "Иванов",
                LastName = "Иван",
                Email = "ivanov@ya.com",
                Password = Hasher.GetHash("test_1", salt),
                Salt = salt,
            }
        };

        var employeeRoles = new EmployeeRole[]
        {
            new EmployeeRole()
            {
                EmployeeId = employees[0].Id,
                RoleId = roles[0].Id,
            }
        };

        builder.Entity<Role>().HasData(roles);
        builder.Entity<Employee>().HasData(employees);
        builder.Entity<EmployeeRole>().HasData(employeeRoles);
    }
}
