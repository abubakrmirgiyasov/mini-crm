using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Models;
using MiniCrm.UI.Repositories.Interfaces;
using MiniCrm.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace MiniCrm.UI.Repositories;

public class ProjectsRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectViewModel>> GetProjectsAsync()
    {
        try
        {
            var projects = await _context.Projects
                .Include(x => x.EmployeeProjects)
                .ThenInclude(x => x.Employee)
                .Include(x => x.Manager)
                .ToListAsync();
            return ExtractingProjectDTO.ExtractingToViewModels(projects);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<IEnumerable<EmployeeProjectsViewModel>> GetSampleProjectsAsync()
    {
        try
        {
            var projects = await _context.Projects.ToListAsync();
            return ExtractingProjectDTO.ExtractingSampleViewModels(projects);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ProjectViewModel> GetProjectByIdAsync(Guid id)
    {
        try
        {
            var project = await _context.Projects
                .Include(x => x.Manager)
                .Include(x => x.EmployeeProjects)
                .ThenInclude(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Проект не найден");
            return ExtractingProjectDTO.ExtractingToViewModel(project);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task CreateProjectAsync(ProjectBindingModel model)
    {
        try
        {
            var project = ExtractingProjectDTO.ExtractingFromBindingModel(model);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task EditProjectAsync(ProjectBindingModel model)
    {
        try
        {
            var project = await _context.Projects
                .Include(x => x.Manager)
                .Include(x => x.EmployeeProjects)
                .ThenInclude(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == model.Id)
                ?? throw new Exception("Проект не найден");

            project.Name = model.Name;
            project.CustomerCompanyInfo = model.CustomerCompany;
            project.PerformingCompanyInfo = model.PerformingCompany;
            project.ExpirationDate = model.ExpirationDate;
            project.Priority = model.Priority;
            project.UpdateDateTime = DateTime.Now;
            project.EmployeeProjects = new List<EmployeeProject>();

            if (project.Manager is not null)
                project.Manager.EmployeeId = model.ManagerId;
            else
                project.Manager = new Manager()
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = model.ManagerId,
                };

            if (model.Employees is not null)
            {
                foreach (var employee in model.Employees)
                {
                    project.EmployeeProjects.Add(new EmployeeProject()
                    {
                        EmployeeId = employee,
                        ProjectId = project.Id,
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task DeleteProjectAsync(Guid id)
    {
        try
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Проект не найден");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
