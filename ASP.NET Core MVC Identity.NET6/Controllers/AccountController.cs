using ASP.NET_Core_MVC_Identity.NET6.Interfaces;
using ASP.NET_Core_MVC_Identity.NET6.Models;
using ASP.NET_Core_MVC_Identity.NET6.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_MVC_Identity.NET6.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ISendGridEmail _sendGridEmail;

    public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ISendGridEmail sendGridEmail)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _sendGridEmail = sendGridEmail;
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

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackurl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            await _sendGridEmail.SendEmailAsync(model.Email, "Reset Email Confirmation", "Please reset email by going to this " +
                "<a href=\"" + callbackurl + "\">link</a>");
            return RedirectToAction("ForgotPasswordConfirmation");
        }
        return View(model);
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
                                                            lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
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

    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
