using ASP.NET_Core_MVC_Identity.NET6.Models;
using ASP.NET_Core_MVC_Identity.NET6.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_MVC_Identity.NET6.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Register(string? returnUrl = null)
    {
        RegisterViewModel registerViewModel = new();
        registerViewModel.ReturnUrl = returnUrl;
        return View(registerViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
    {
        registerViewModel.ReturnUrl = returnUrl;
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = new AppUser { Email = registerViewModel.Email, UserName = registerViewModel.UserName };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            ModelState.AddModelError("Password", "User could not be created. Password not unique enough");
        }

        return View(registerViewModel);
    }
}
