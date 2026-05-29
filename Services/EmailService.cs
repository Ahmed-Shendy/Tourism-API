
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
namespace Tourism_Api.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("EDuSmart.ai", _config["EmailSettings:From"]));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        var isHtmlMessage = message.Contains("<html", StringComparison.OrdinalIgnoreCase)
            || message.Contains("<body", StringComparison.OrdinalIgnoreCase)
            || message.Contains("<a ", StringComparison.OrdinalIgnoreCase);

        emailMessage.Body = new TextPart(isHtmlMessage ? "html" : "plain")
        {
            Text = message
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["EmailSettings:SmtpServer"],
                                 int.Parse(_config["EmailSettings:Port"]),
                                 SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_config["EmailSettings:Username"],
                                     _config["EmailSettings:Password"]);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}
