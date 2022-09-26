using ASP.NET_Core_MVC_Identity.NET6.Data;
using ASP.NET_Core_MVC_Identity.NET6.Models;
using ASP.NET_Core_MVC_Identity.NET6.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ASP.NET_Core_MVC_Identity.NET6.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public UserController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [Authorize]
    public IActionResult Index()
    {
        var userList = _db.AppUser.ToList();
        var rolesList = _db.UserRoles.ToList();
        var roles = _db.Roles.ToList();
        foreach (var user in userList)
        {
            var role = rolesList.FirstOrDefault(x => x.UserId == user.Id);
            if (role == null)
            {
                user.Role = "None";
            }
            else
            {
                user.Role = roles.FirstOrDefault(x => x.Id == role.RoleId).Name;
            }
        }

        return View(userList);
    }

    [Authorize]
    public IActionResult Edit(string userId)
    {
        var user = _db.AppUser.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return NotFound();
        }
        var userRole = _db.UserRoles.ToList();
        var roles = _db.Roles.ToList();
        var role = userRole.FirstOrDefault(u => u.UserId == user.Id);
        if (role != null)
        {
            user.RoleId = roles.FirstOrDefault(u => u.Id == role.RoleId).Id;
        }
        user.RoleList = _db.Roles.Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id
        });
        return View(user);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AppUser user)
    {
        if (ModelState.IsValid)
        {
            var userDbValue = _db.AppUser.FirstOrDefault(x => x.Id == user.Id);
            if (userDbValue == null)
            {
                return NotFound();
            }
            var userRole = _db.UserRoles.FirstOrDefault(x => x.UserId == userDbValue.Id);
            if (userRole != null)
            {
                var previousRoleName = _db.Roles.Where(x => x.Id == userRole.RoleId).Select(x => x.Name).FirstOrDefault();
                await _userManager.RemoveFromRoleAsync(userDbValue, previousRoleName);
            }
            await _userManager.AddToRoleAsync(userDbValue, _db.Roles.FirstOrDefault(x => x.Id == user.RoleId).Name);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        user.RoleList = _db.Roles.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id
        });

        return View(user);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Delete(string userId)
    {
        var user = _db.AppUser.FirstOrDefault(x => x.Id == userId);
        if (user == null)
        {
            return NotFound();
        }
        _db.AppUser.Remove(user);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> ManageClaims(string userId)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        IList<Claim> existingUserClaims = await _userManager.GetClaimsAsync(user);

        UserClaimsViewModel model = new()
        {
            UserId = userId
        };

        foreach (Claim claim in ClaimStore.claimsList)
        {
            UserClaim userClaim = new()
            {
                ClaimType = claim.Type
            };
            if (existingUserClaims.Any(c => c.Type == claim.Type))
            {
                userClaim.IsSelected = true;
            }
            model.Claims.Add(userClaim);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManageClaims(UserClaimsViewModel userClaimsViewModel)
    {
        var user = await _userManager.FindByIdAsync(userClaimsViewModel.UserId);

        if (user == null)
        {
            return NotFound();
        }

        var claims = await _userManager.GetClaimsAsync(user);
        var result = await _userManager.RemoveClaimsAsync(user, claims);

        if (!result.Succeeded)
        {
            return View(userClaimsViewModel);
        }

        result = await _userManager.AddClaimsAsync(user,
            userClaimsViewModel.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.IsSelected.ToString()))
            );

        if (!result.Succeeded)
        {
            return View(userClaimsViewModel);
        }

        return RedirectToAction(nameof(Index));
    }

}
