using MiniCrm.UI.Models.DTO_s;

namespace MiniCrm.UI.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<ProjectViewModel>> GetProjectsAsync();

    Task<IEnumerable<EmployeeProjectsViewModel>> GetSampleProjectsAsync();

    Task<ProjectViewModel> GetProjectByIdAsync(Guid id);

    Task CreateProjectAsync(ProjectBindingModel model);

    Task EditProjectAsync(ProjectBindingModel model);

    Task DeleteProjectAsync(Guid id);
}
