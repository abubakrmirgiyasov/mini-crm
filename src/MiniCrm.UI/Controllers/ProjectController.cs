using Microsoft.AspNetCore.Mvc;
using MiniCrm.UI.Models;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Repositories.Interfaces;

namespace MiniCrm.UI.Controllers;

public class ProjectController : Controller
{
    private readonly IProjectRepository _project;
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(
        IProjectRepository project,
        ILogger<ProjectController> logger)
    {
        _project = project;
        _logger = logger;
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
            _logger.LogError("Getting all projects. Error {Message}", ex.Message);
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
            return View();
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            _logger.LogError("Adding project. Error {Message}", ex.Message);
            return BadRequest(ex.Message);
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
            return View();
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
            return View();
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }
    
    public async Task<IActionResult> GetSampleProjects()
    {
        try
        {
            var projects = await _project.GetSampleProjectsAsync();
            return Json(projects);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }
}
