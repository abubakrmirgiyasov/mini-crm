namespace MiniCrm.UI.Models.DTO_s;

public class ProjectBindingModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public string CustomerCompany { get; set; } = null!;

    public string PerformingCompany { get; set; } = null!;

    public int Priority { get; set; }

    public DateTimeOffset ExpirationDate { get; set; }

    public Guid ManagerId { get; set; }

    public Guid[] Employees { get; set; } = null!;
}

public record ProjectViewModel(
    Guid? Id = null,
    string? Name = null,
    string? CustomerCompany = null,
    string? PerformingCompany = null,
    DateTimeOffset? CreationDate = null,
    DateTimeOffset? ExpirationDate = null,
    int? Priority = null,
    Guid? ManagerId = null,
    ManagerViewModel? Manager = null,
    List<EmployeeProjectsViewModel>? Employees = null);

public class ExtractingProjectDTO
{
    public static IEnumerable<ProjectViewModel> ExtractingToViewModels(List<Project> models)
    {
        var projects = new List<ProjectViewModel>();

        foreach (var project in models)
        {
            var current = new ProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                CustomerCompany = project.CustomerCompanyInfo,
                PerformingCompany = project.PerformingCompanyInfo,
                ExpirationDate = project.ExpirationDate,
                Priority = project.Priority,
                Employees = new List<EmployeeProjectsViewModel>(),
                Manager = new ManagerViewModel()
                {
                    Value = project.Manager?.Employee?.Id,
                    Label = project.Manager?.Employee?.FirstName,
                },
            };

            foreach (var model in project.EmployeeProjects)
            {
                current.Employees.Add(new EmployeeProjectsViewModel()
                {
                    Label = $"{model.Employee?.FirstName} {model.Employee?.LastName}",
                    Value = model.Employee?.Id,
                });
            }

            projects.Add(current);
        }

        return projects;
    }

    public static Project ExtractingFromBindingModel(ProjectBindingModel model)
    {
        var project = new Project()
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            CustomerCompanyInfo = model.CustomerCompany,
            PerformingCompanyInfo = model.PerformingCompany,
            ExpirationDate = model.ExpirationDate,
            Priority = model.Priority,
            EmployeeProjects = new List<EmployeeProject>(),
            Manager = new Manager()
            {
                Id = Guid.NewGuid(),
                EmployeeId = model.ManagerId,
            },
        };

        if (model.Employees is not null)
        {
            foreach (var employeeProject in model.Employees)
            {
                project.EmployeeProjects.Add(new EmployeeProject()
                {
                    EmployeeId = employeeProject,
                    ProjectId = project.Id,
                });
            }
        }

        return project;
    }

    public static ProjectViewModel ExtractingToViewModel(Project model)
    {
        var project = new ProjectViewModel()
        {
            Id = model.Id,
            Name = model.Name,
            CustomerCompany = model.CustomerCompanyInfo,
            PerformingCompany = model.PerformingCompanyInfo,
            ExpirationDate = model.ExpirationDate,
            Priority = model.Priority,
            Employees = new List<EmployeeProjectsViewModel>(),
            Manager = new ManagerViewModel()
            {
                Value = model.Manager?.Employee?.Id,
                Label = model.Manager?.Employee?.FirstName,
            },
        };

        foreach (var employee in model.EmployeeProjects)
        {
            project.Employees.Add(new EmployeeProjectsViewModel()
            {
                Label = employee.Project?.Name,
                Value = employee.Project?.Id,
            });
        }

        return project;
    }

    public static IEnumerable<EmployeeProjectsViewModel> ExtractingSampleViewModels(List<Project> models)
    {
        var employees = new List<EmployeeProjectsViewModel>();

        foreach (var employee in models)
        {
            employees.Add(new EmployeeProjectsViewModel()
            {
                Value = employee.Id,
                Label = employee.Name,
            });
        }

        return employees;
    }
}
