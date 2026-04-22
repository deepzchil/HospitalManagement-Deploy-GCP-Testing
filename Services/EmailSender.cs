using Microsoft.AspNetCore.Identity.UI.Services;

namespace HospitalManagementSystem.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // For development only (no real email sending)
            Console.WriteLine($"Email to: {email} | Subject: {subject}");
            return Task.CompletedTask;
        }
    }
}