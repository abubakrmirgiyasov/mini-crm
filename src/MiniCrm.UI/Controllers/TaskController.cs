using Microsoft.AspNetCore.Mvc;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Repositories.Interfaces;
using MiniCrm.UI.Services;

namespace MiniCrm.UI.Controllers;

[Authorize]
public class TaskController : Controller
{
    private readonly ITaskRepository _task;
    private readonly ILogger<TaskController> _logger;

    public TaskController(
        ITaskRepository task,
        ILogger<TaskController> logger)
    {
        _task = task;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string sortOrder)
    {
        try
        {
            ViewBag.NameSortParam = !string.IsNullOrEmpty(sortOrder) ? "NameSortParam" : "";
            ViewBag.AuthorSortParam = !string.IsNullOrEmpty(sortOrder) ? "AuthorSortParam" : ""; 
            ViewBag.AuthorSortParam = !string.IsNullOrEmpty(sortOrder) ? "ExecutorSortParam" : ""; 

            var tasks = await _task.GetTasksAsync();

            tasks = sortOrder switch
            {
                "NameSortParam" => tasks.OrderByDescending(x => x.Name),
                "AuthorSortParam" => tasks.OrderByDescending(x => x.AuthorId),
                "ExecutorSortParam" => tasks.OrderByDescending(x => x.ExecutorId),
                _ => tasks.OrderByDescending(x => x.Priority),
            };

            return View(tasks);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "project_manager,manager")]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "project_manager,manager")]
    public async Task<IActionResult> Create([FromForm] TaskBindingModel model)
    {
        try
        {
            await _task.CreateTaskAsync(model);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "project_manager,manager")]
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var task = await _task.GetTaskByIdAsync(id);
            return View(task);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = "project_manager,manager")]
    public async Task<IActionResult> Edit(Guid id, [FromForm] TaskBindingModel model)
    {
        try
        {
            model.Id = id;
            await _task.EditTaskAsync(model);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "project_manager,manager")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var task = await _task.GetTaskByIdAsync(id);
            return View(task);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize(Roles = "project_manager,manager")]
    public async Task<IActionResult> Delete(Guid id, IFormCollection? collection)
    {
        try
        {
            await _task.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ChangeTaskStatus([FromForm] TaskStatusChangeBindingModel model)
    {
        try
        {
            await _task.ChangeTaskStatusAsync(model);
            return View();
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> GetSampleTasks()
    {
        try
        {
            var tasks = await _task.GetSampleTasksAsync();
            return Json(tasks);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }
}
