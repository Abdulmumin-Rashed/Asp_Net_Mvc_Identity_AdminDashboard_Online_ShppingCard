using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Dashboard.Models;

namespace Dashboard.Data.Services
{
  

    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential
                {
                    UserName = _emailSettings.UserName,
                    Password = _emailSettings.Password
                };

                client.Credentials = credentials;
                client.Host = _emailSettings.MailServer;
                client.Port = _emailSettings.MailPort;
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(email);
                    emailMessage.Subject = subject;
                    emailMessage.Body = message;
                    emailMessage.From = new MailAddress(_emailSettings.Sender);

                    await client.SendMailAsync(emailMessage);
                }
            }
        }
    }

}
