using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace Squirrel.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration) => _configuration = configuration;
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_configuration["EmailConfiguration:UserName"],
                                                     _configuration["EmailConfiguration:From"]));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration["EmailConfiguration:SmtpServer"],
                                      int.Parse(_configuration["EmailConfiguration:Port"]),
                                      useSsl: true);
            client.Authenticate(_configuration["EmailConfiguration:From"],
                                _configuration["EmailConfiguration:Password"]);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
