using ASP.NET_Core_MVC_Identity.NET6.Models;
using ASP.NET_Core_MVC_Identity.NET6.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_MVC_Identity.NET6.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        LoginViewModel loginViewModel = new()
        {
            ReturnUrl = returnUrl ?? Url.Content("~/")
        };
        return View(loginViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                                                            loginViewModel.Password,
                                                            loginViewModel.RememberMe,
                                                            lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View(loginViewModel);
            }
        }
        return View(loginViewModel);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
