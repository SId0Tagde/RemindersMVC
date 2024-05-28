
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Reminder.Models;
using Microsoft.CodeAnalysis.Options;

namespace Reminder.Services
{
    public class SmtpEmailService : IEmailService
    {
        #region Private Variables.
        private readonly EmailConfiguration _configuration;
        #endregion

        #region Constructor.
        public SmtpEmailService(IOptions<EmailConfiguration> options)
        {
            _configuration = options.Value;   
        }
        #endregion

        #region Public Methods.
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient
            {
                Host = _configuration.host, // e.g., "smtp.example.com"
                Port = _configuration.port, // or your SMTP port
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration.Username, _configuration.Password), // your SMTP credentials
                EnableSsl = _configuration.EnableSsl // enable SSL if required by your SMTP server
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration.FromEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true // set to true if your message is HTML
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
        #endregion
    }
}
