using Microsoft.AspNetCore.Mvc;
using MiniCrm.UI.Models;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Repositories.Interfaces;
namespace MiniCrm.UI.Controllers;

public class ProjectController : Controller
{
    private readonly ILogger<ProjectController> _logger;
    private readonly IProjectRepository _project;
    private readonly IEmployeeRepository _employee;
    private readonly IManagerRepository _manager;

    public ProjectController(
        IEmployeeRepository employee,
        IProjectRepository project,
        IManagerRepository manager,
        ILogger<ProjectController> logger)
    {
        _employee = employee;
        _project = project;
        _logger = logger;
        _manager = manager;
    }

    public async Task<IActionResult> Index(string sortOrder)
    {
        try
        {
            ViewBag.NameSortParam = !string.IsNullOrEmpty(sortOrder) ? "NameSortParam" : "";
            ViewBag.NamePerformParam = !string.IsNullOrEmpty(sortOrder) ? "NamePerformParam" : "";

            var projects = await _project.GetProjectsAsync();

            projects = sortOrder switch
            {
                "NameSortParam" => projects.OrderByDescending(x => x.Name),
                "NamePerformParam" => projects.OrderByDescending(x => x.PerformingCompany),
                "StatusSort" => projects.OrderByDescending(x => x.Priority),
                _ => projects.OrderBy(x => x.Priority),
            };

            _logger.LogInformation("Getting all projects, founded {Count}", projects.Count());

            return View(projects);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Getting all projects. Error {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] ProjectBindingModel model)
    {
        try
        {
            await _project.CreateProjectAsync(model);
            return RedirectToPage("/project/index");
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return View();
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var project = await _project.GetProjectByIdAsync(id);
            return View(project);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [FromForm] ProjectBindingModel model)
    {
        try
        {
            model.Id = id;
            await _project.EditProjectAsync(model);
            return RedirectToRoute("/");
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var project = await _project.GetProjectByIdAsync(id);
            return View(project);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id, IFormCollection? form)
    {
        try
        {
            await _project.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    public async Task<ActionResult> GetEmployees()
    {
        try
        {
            var employees = await _employee.GetSampleEmployeesAsync();
            return Json(employees);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    public async Task<ActionResult> GetManagers()
    {
        try
        {
            var managers = await _manager.GetSampleManagersAsync();
            return Json(managers);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }
}
