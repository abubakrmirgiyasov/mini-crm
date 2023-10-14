using System.Collections.Generic;

namespace MiniCrm.UI.Models.DTO_s;

public class EmployeeBindingModel
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid[]? Projects { get; set; }
}

public record EmployeeViewModel(
    Guid? Id = null,
    string? FirstName = null,
    string? LastName = null,
    string? Email = null,
    List<EmployeeProjectsViewModel>? Projects = null);

public class ExtractingEmployeeDTO
{
    public static List<EmployeeViewModel> ExtractingToViewModels(List<Employee> models)
    {
        var employees = new List<EmployeeViewModel>();

        foreach (var employee in models)
        {
            var single = new EmployeeViewModel()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Projects = new List<EmployeeProjectsViewModel>(),
            };

            foreach (var project in employee.EmployeeProjects)
            {
                single.Projects.Add(new EmployeeProjectsViewModel()
                {
                    Label = project.Project?.Name,
                    Value = project.Project?.Id,
                });
            }

            employees.Add(single);
        }

        return employees;
    }

    public static Employee ExtractingFromBindingModel(EmployeeBindingModel model)
    {
        var employee = new Employee()
        {
            Id = Guid.NewGuid(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email.Trim(),
            EmployeeProjects = new List<EmployeeProject>(),
        };

        if (model.Projects is not null)
        {
            foreach (var project in model.Projects)
            {
                employee.EmployeeProjects.Add(new EmployeeProject()
                {
                    EmployeeId = employee.Id,
                    ProjectId = project,
                });
            }
        }

        return employee;
    }

    public static EmployeeViewModel ExtractingToViewModel(Employee model)
    {
        var employee = new EmployeeViewModel()
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Projects = new List<EmployeeProjectsViewModel>(),
        };

        if (model.EmployeeProjects is not null)
        {
            foreach (var project in model.EmployeeProjects)
            {
                employee.Projects.Add(new EmployeeProjectsViewModel()
                {
                    Label = project.Project?.Name,
                    Value = project.Project?.Id,
                });
            }
        }

        return employee;
    }

    public static IEnumerable<EmployeeProjectsViewModel> ExtractingSampleViewModels(List<Employee> models)
    {
        var employees = new List<EmployeeProjectsViewModel>();

        foreach (var employee in models)
        {
            employees.Add(new EmployeeProjectsViewModel()
            {
                Value = employee.Id,
                Label = $"{employee.FirstName} {employee.LastName}",
            });
        }

        return employees;
    }
}
