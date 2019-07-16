using CarOwnershipWebApp.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Services
{
    public class EmailService : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                SmtpClient client = new SmtpClient(_emailSettings.SmtpClient);
                client.Port = _emailSettings.Port;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                MailMessage mailMessage = new MailMessage(_emailSettings.Username, email, subject, htmlMessage);
                mailMessage.IsBodyHtml = true;
                client.Send(mailMessage);

                return Task.CompletedTask;
            }

            catch (Exception ex)
            {
                //TO-DO: logger
                return Task.FromException(ex);
            }
        }
    }
}
