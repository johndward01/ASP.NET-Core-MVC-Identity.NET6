using System.Security.Claims;

namespace ASP.NET_Core_MVC_Identity.NET6.Models;

public class ClaimStore
{
    public static List<Claim> claimsList = new()
    {
        new Claim("Create", "Create"),
        new Claim("Edit", "Edit"),
        new Claim("Delete", "Delete")
    };
}
