using Microsoft.AspNetCore.Identity.UI.Services;

namespace UdemyKitapSitesi.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //EMAİL GÖNDERME İŞLEMLERİ BURAYA YAPILIR
            return Task.CompletedTask;
        }
    }
}
