using ASP.NET_Core_MVC_Identity.NET6.Data;
using ASP.NET_Core_MVC_Identity.NET6.Models;
using ASP.NET_Core_MVC_Identity.NET6.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Permissions;

namespace ASP.NET_Core_MVC_Identity.NET6.Controllers;
public class RoleController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public RoleController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _dbContext = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    public IActionResult Index()
    {
        var roles = _dbContext.Roles.ToList();
        return View(roles);
    }

    [HttpGet]
    public IActionResult Upsert(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return View();  
        }
        else
        {
            var user = _dbContext.Roles.FirstOrDefault(x => x.Id == id);
            return View(user);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(IdentityRole role)
    {
        if (await _roleManager.RoleExistsAsync(role.Name))
        {
            return RedirectToAction("Index");
        }
        if (string.IsNullOrEmpty(role.Id))
        {
            await _roleManager.CreateAsync(new IdentityRole() { Name = role.Name });
        }
        else
        {
            var roleDb = _dbContext.Roles.FirstOrDefault(x => x.Id == role.Id);
            if (roleDb == null)
            {
                return RedirectToAction(nameof(Index));
            }
            roleDb.Name = role.Name;
            roleDb.NormalizedName = role.Name.ToUpper();
            var result = await _roleManager.UpdateAsync(roleDb);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var roleDb = _dbContext.Roles.FirstOrDefault(x => x.Id == id);
        if (roleDb == null)
        {
            return RedirectToAction(nameof(Index));
        }
        var userRolesForThisRole = _dbContext.UserRoles.Where(x => x.RoleId == id).Count();
        if (userRolesForThisRole > 0)
        {
            return RedirectToAction(nameof(Index));
        }
        await _roleManager.DeleteAsync(roleDb);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> ManageClaims(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var existingUserClaims = await _userManager.GetClaimsAsync(user);

        var model = new UserClaimsViewModel()
        {
            UserId = userId
        };

        foreach (Claim claim in ClaimStore.claimsList)
        {
            UserClaim userClaim = new UserClaim
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
