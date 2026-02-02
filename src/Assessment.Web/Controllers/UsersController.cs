using Assessment.Application.Dtos.Auth;
using Assessment.Application.Security;
using Assessment.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Web.Controllers;

[Authorize(Policy = AppPolicies.ProvidersManage)] // Admins only
public class UsersController : Controller
{
    private readonly UserAdminService _svc;
    public UsersController(UserAdminService svc) { _svc = svc; }

    [HttpGet("/Users/Create")]
    public IActionResult Create()
        => View(new CreateUserRequestDto());

    [HttpPost("/Users/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserRequestDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _svc.CreateUserAsync(dto, ct);
            TempData["Msg"] = "User created.";
            return RedirectToAction("Login", "Account");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }
}

