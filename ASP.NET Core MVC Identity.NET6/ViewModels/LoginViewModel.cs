using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Identity.NET6.ViewModels;

public class LoginViewModel
{
    [Required]
    [DataType(DataType.Password)]    
    public string Password { get; set; }

    [Display(Name = "Remember Me?")]
    public bool RememberMe { get; set; }
    public string UserName { get; set; }
    public string? ReturnUrl { get; set; }
}
