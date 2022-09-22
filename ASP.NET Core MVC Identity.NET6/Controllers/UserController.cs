using ASP.NET_Core_MVC_Identity.NET6.Data;
using ASP.NET_Core_MVC_Identity.NET6.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NET_Core_MVC_Identity.NET6.Controllers;
public class UserController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;

    public UserController(ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

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



}
