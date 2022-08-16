using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_MVC_Identity.NET6.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}
