using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookingApp.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.provider.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("you@domain.com", "password"),
                EnableSsl = true
            };
            return client.SendMailAsync("you@domain.com", email, subject, htmlMessage);
        }
    }
}
