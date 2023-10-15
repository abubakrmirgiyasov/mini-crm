﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MiniCrm.UI.Models;
using MiniCrm.UI.Repositories.Interfaces;
using MiniCrm.UI.Services;
using System.Diagnostics;

namespace MiniCrm.UI.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProjectRepository _project;

    public HomeController(IProjectRepository project, ILogger<HomeController> logger)
    {
        _logger = logger;
        _project = project;
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
