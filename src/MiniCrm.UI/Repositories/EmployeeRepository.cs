using Microsoft.VisualBasic;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Models;
using MiniCrm.UI.Repositories.Interfaces;
using MiniCrm.UI.Services;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;
using MiniCrm.UI.Common;

namespace MiniCrm.UI.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync()
    {
        try
        {
            var employees = await _context.Employees
                .Include(x => x.Managers)
                .Include(x => x.EmployeeProjects)
                .ThenInclude(x => x.Project)
                .ToListAsync();

            return ExtractingEmployeeDTO.ExtractingToViewModels(employees);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<IEnumerable<EmployeeProjectsViewModel>> GetSampleEmployeesAsync()
    {
        try
        {
            var employees = await _context.Employees.ToListAsync();
            return ExtractingEmployeeDTO.ExtractingSampleViewModels(employees);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<EmployeeViewModel> GetEmployeeByIdAsync(Guid id)
    {
        try
        {
            var employees = await _context.Employees
                .Include(x => x.Managers)
                .Include(x => x.EmployeeProjects)
                .ThenInclude(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Сотрудник не найден");
            return ExtractingEmployeeDTO.ExtractingToViewModel(employees);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task CreateEmployeeAsync(EmployeeBindingModel model)
    {
        try
        {
            var employee = ExtractingEmployeeDTO.ExtractingFromBindingModel(model);

            var role = await _context.Roles
                .FirstOrDefaultAsync(x => x.Name == model.Role)
                ?? throw new Exception("При добавлении возникла ошибка");

            employee.EmployeeRoles.Add(new EmployeeRole()
            {
                RoleId = role.Id,
                EmployeeId = employee.Id,
            });

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task EditEmployeeAsync(EmployeeBindingModel model)
    {
        try
        {
            var employee = await _context.Employees
                .Include(x => x.EmployeeProjects)
                .ThenInclude(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == model.Id)
                ?? throw new Exception("Сотрудник не найден");

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.UpdateDateTime = DateTime.Now;
            employee.Email = model.Email;
            employee.Password = Hasher.GetHash(model.Password, employee.Salt);
            employee.EmployeeProjects = new List<EmployeeProject>();

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

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task DeleteEmployeeAsync(Guid id)
    {
        try
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Проект не найден");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
