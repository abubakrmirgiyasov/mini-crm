using Microsoft.EntityFrameworkCore;
using MiniCrm.UI.Common;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Repositories.Interfaces;
using MiniCrm.UI.Services;

namespace MiniCrm.UI.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskViewModel>> GetTasksAsync()
    {
        try
        {
            var tasks = await _context.Tasks
                .Include(x => x.Author)
                .Include(x => x.Executor)
                .Include(x => x.Project)
                .ToListAsync();
            return ExtractingTaskDTO.FormingToViewModels(tasks);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<IEnumerable<TaskSampleViewModel>> GetSampleTasksAsync()
    {
        try
        {
            var tasks = await _context.Tasks.ToListAsync();
            return ExtractingTaskDTO.ExtractingSampleViewModels(tasks);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<TaskViewModel> GetTaskByIdAsync(Guid id)
    {
        try
        {
            var task = await _context.Tasks
                .Include(x => x.Author)
                .Include(x => x.Executor)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Задача не найдена");
            return ExtractingTaskDTO.ExtractingToViewModel(task);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task CreateTaskAsync(TaskBindingModel model)
    {
        try
        {
            var task = ExtractingTaskDTO.ExtractingFromBindingModel(model);

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task EditTaskAsync(TaskBindingModel model)
    {
        try
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(x => x.Id == model.Id)
                ?? throw new Exception("Задача не найдена");
            task.AuthorId = model.AuthorId;
            task.ProjectId = model.ProjectId;
            task.ExecutorId = model.ExecutorId;
            task.Name = model.Name;
            task.Description = model.Description;
            task.UpdateDateTime = DateTimeOffset.Now;
            task.Status = model.Status;
            task.Priority = model.Priority;

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        try
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Задача не найдена");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task ChangeTaskStatusAsync(TaskStatusChangeBindingModel model)
    {
        try
        {
            var id = model.Status.Split("_");

            if (Guid.TryParse(id.LastOrDefault(), out Guid taskId))
            {
                var task = await _context.Tasks
                    .FirstOrDefaultAsync(x => x.Id == taskId)
                    ?? throw new Exception("Произошла ошибка при изменении статуса");

                task.Status = Enum.Parse<Status>(id.FirstOrDefault()!);
         
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
