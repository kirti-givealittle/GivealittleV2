using GivealittleV2.Application.Interfaces.Email;
using GivealittleV2.Domain.Models.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading;

public sealed class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly IEmailTemplateRenderer emailTemplateRenderer;

    public SmtpEmailService(IOptions<EmailSettings> options , IEmailTemplateRenderer _emailTemplateRenderer)
    {
        _settings = options.Value;
        emailTemplateRenderer = _emailTemplateRenderer;
    }

    public async Task SendAsync(
        string to,
        string subject,
        string body,
        bool isHtml = true,
        CancellationToken cancellationToken = default)
    {
        using var client = new SmtpClient(_settings.SmtpServer, _settings.Port)
        {
            Credentials = new NetworkCredential(
                _settings.Username,
                _settings.Password),
            EnableSsl = _settings.UseSsl
        };

        var message = new MailMessage
        {
            From = new MailAddress(
                _settings.SenderEmail,
                _settings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };

        message.To.Add(to);

        await client.SendMailAsync(message);
    }

    public async Task<string> RenderOTPTemplate(string UserName, string OTP, int Minutes)
    {
        var bodyHtml = await emailTemplateRenderer.RenderAsync(
             templateName: "Otp",
            model: new Dictionary<string, object?>
            {
                ["UserName"] = UserName,
                ["Otp"] = OTP,
                ["Minutes"] = Minutes
            }
           );
        return bodyHtml;
    }

       
}
