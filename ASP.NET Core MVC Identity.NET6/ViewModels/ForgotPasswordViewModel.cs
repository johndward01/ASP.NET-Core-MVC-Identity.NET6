using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Identity.NET6.ViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

}
