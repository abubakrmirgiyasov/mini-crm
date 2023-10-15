using MiniCrm.UI.Common;
using System.Threading.Tasks;

namespace MiniCrm.UI.Models.DTO_s;

public class TaskBindingModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Priority { get; set; } = 1;

    public Status Status { get; set; }

    public Guid AuthorId { get; set; }

    public Guid ProjectId { get; set; }

    public Guid? ExecutorId { get; set; }
}

public record TaskViewModel(
    Guid? Id = null,
    string? Name = null,
    string? Description = null,
    int? Priority = null,
    string? Status = null,
    string? AuthorId = null,
    string? ExecutorId = null,
    string? ProjectId = null);

public record TaskSampleViewModel(
    Guid? Value = null,
    string? Label = null);

public class TaskStatusChangeBindingModel
{
    public string Status { get; set; } = null!;
}

public class ExtractingTaskDTO
{
    public static Task ExtractingFromBindingModel(TaskBindingModel model)
    {
        return new Task()
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Status = model.Status,
            AuthorId = model.AuthorId,
            ExecutorId = model.ExecutorId,
            Description = model.Description,
            Priority = model.Priority,
            ProjectId = model.ProjectId,
        };
    }

    public static IEnumerable<TaskViewModel> FormingToViewModels(List<Task> models)
    {
        var tasks = new List<TaskViewModel>();

        foreach (var task in models)
        {
            tasks.Add(new TaskViewModel()
            {
                Id = task.Id,
                Name = task.Name,
                Priority = task.Priority,
                Description = task.Description,
                AuthorId = $"{task.Author.FirstName} {task.Author.LastName}",
                ExecutorId = $"{task.Executor?.FirstName} {task.Executor?.LastName}",
                ProjectId = task.Project.Name,
                Status = task.Status.ToString(),
            });
        }

        return tasks;
    }

    public static TaskViewModel ExtractingToViewModel(Task model)
    {
        return new TaskViewModel()
        {
            Id = model.Id,
            Name = model.Name,
            Priority = model.Priority,
            Description = model.Description,
            Status = model.Status.ToString(),
            AuthorId = $"{model.Author.FirstName} {model.Author.LastName}",
            ExecutorId = $"{model.Executor?.FirstName} {model.Executor?.LastName}",
            ProjectId = model.Project.Name,
        };
    }

    public static IEnumerable<TaskSampleViewModel> ExtractingSampleViewModels(List<Task> models)
    {
        var tasks = new List<TaskSampleViewModel>();

        foreach (var task in models)
        {
            tasks.Add(new TaskSampleViewModel()
            {
                Value = task.Id,
                Label = task.Name,
            });
        }

        return tasks;
    }
}
