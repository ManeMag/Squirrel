using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace Squirrel.Services
{
    public class EmailConfiguration
    {   
        public string? From { get; set; }
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration emailConfig;
        public EmailSender(EmailConfiguration emailConfig) => this.emailConfig = emailConfig;
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(emailConfig.UserName, emailConfig.From));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, true);
            client.Authenticate(emailConfig.UserName, emailConfig.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
