using MiniCrm.UI.Models.DTO_s;

namespace MiniCrm.UI.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<TaskViewModel>> GetTasksAsync();

    Task<IEnumerable<TaskSampleViewModel>> GetSampleTasksAsync();

    Task<TaskViewModel> GetTaskByIdAsync(Guid id);

    Task CreateTaskAsync(TaskBindingModel model);

    Task EditTaskAsync(TaskBindingModel model);

    Task DeleteTaskAsync(Guid id);

    Task ChangeTaskStatusAsync(TaskStatusChangeBindingModel model);
}
