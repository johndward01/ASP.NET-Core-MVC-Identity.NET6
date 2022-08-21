namespace ASP.NET_Core_MVC_Identity.NET6.Interfaces;

public interface ISendGridEmail
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}
