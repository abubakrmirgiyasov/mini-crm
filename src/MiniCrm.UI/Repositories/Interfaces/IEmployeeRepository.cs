using MiniCrm.UI.Models.DTO_s;

namespace MiniCrm.UI.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync();

    Task<IEnumerable<EmployeeProjectsViewModel>> GetSampleEmployeesAsync();

    Task<EmployeeViewModel> GetEmployeeByIdAsync(Guid id);

    Task CreateEmployeeAsync(EmployeeBindingModel model);

    Task EditEmployeeAsync(EmployeeBindingModel model);

    Task DeleteEmployeeAsync(Guid id);
}
