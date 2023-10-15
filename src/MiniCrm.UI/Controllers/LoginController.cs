using Microsoft.AspNetCore.Mvc;
using MiniCrm.UI.Extensions;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Repositories.Interfaces;
using MiniCrm.UI.Services;

namespace MiniCrm.UI.Controllers;

[AllowAnonymous]
public class LoginController : Controller
{
    private readonly ILoginRepository _login;

    public LoginController(ILoginRepository login)
    {
        _login = login;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromForm] SignInBindingModel model)
    {
        try
        {
            var token = await _login.LoginAsync(model);
            Response.SetTokenCookie($"Bearer {token.AccessToken}");
            return Json(token);
        }
        catch (Exception ex)
        {
            ViewData["Error"] = ex.Message;
            return BadRequest(ex.Message);
        }
    }

    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return RedirectToAction(nameof(Index));
    }
}
