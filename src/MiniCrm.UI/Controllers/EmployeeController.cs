using Microsoft.AspNetCore.Mvc;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Models;
using MiniCrm.UI.Repositories.Interfaces;

namespace MiniCrm.UI.Controllers;
public class EmployeeController : Controller
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IEmployeeRepository _employee;
    private readonly IProjectRepository _project;

    public EmployeeController(
        IProjectRepository project,
        IEmployeeRepository employee,
        ILogger<EmployeeController> logger)
    {
        _project = project;
        _employee = employee;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employee.GetEmployeesAsync();
        return View(employees);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] EmployeeBindingModel model)
    {
        try
        {
            await _employee.CreateEmployeeAsync(model);
            return View();
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            _logger.LogError(nameof(Employee), ex.Message);
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var employee = await _employee.GetEmployeeByIdAsync(id);
            return View(employee);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Edit(Guid id, [FromForm] EmployeeBindingModel model)
    {
        try
        {
            model.Id = id;
            await _employee.EditEmployeeAsync(model);
            return RedirectToAction(nameof(Index));
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
            var employee = await _employee.GetEmployeeByIdAsync(id);
            return View(employee);
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
            await _employee.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    public async Task<ActionResult> GetProjects()
    {
        try
        {
            var projects = await _project.GetSampleProjectsAsync();
            return Json(projects);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
