using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Core_MVC_Identity.NET6.Models;

public class AppUser : IdentityUser
{
    [NotMapped]
    public string? RoleId { get; set; }

    [NotMapped]
    public string? Role { get; set; }

    [NotMapped]
    public IEnumerable<SelectListItem>? RoleList { get; set; }
}
