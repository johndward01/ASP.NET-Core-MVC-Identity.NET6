using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_MVC_Identity.NET6.Controllers;

[Authorize]
public class AccessController : Controller
{
    // Authorize from cookie/jwt
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public IActionResult UserAccess()
    {
        throw new NotImplementedException();
    }
}
