using GivealittleV2.Application.Interfaces.Email;
using GivealittleV2.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Infrastructure.Repositories.Email
{
    public sealed class EmailSender : IEmailSender
    {
        private readonly ApplicationDbContext _db;
        private readonly IEnumerable<IEmailProviderSender> _providerSenders;
        private readonly IEmailTemplateRenderer _renderer;
        

        public EmailSender(
            IEnumerable<IEmailProviderSender> providerSenders,
            IEmailTemplateRenderer renderer,
            ApplicationDbContext db)
        {
            _providerSenders = providerSenders;
            _renderer = renderer;
            _db = db;
        }

        public async Task SendAsync(EmailSendRequestDTO req)
        {
            // 1) template
            var template = await GetByKeyAsync(req.TemplateKey)
                ?? throw new InvalidOperationException($"Template not found: {req.TemplateKey}");

            if (!template.IsActive)
                throw new InvalidOperationException($"Template disabled: {req.TemplateKey}");

           var profile = await GetProfileByKeyOrDefaultAsync()
                ?? throw new InvalidOperationException($"Sender details not found");

           
            // 4) render

            var subject = _renderer.Render(template.SubjectTemplate, req.EmailDTO);
            var body = _renderer.Render(template.BodyTemplate, req.EmailDTO);

            var sender = _providerSenders.FirstOrDefault(x =>
                string.Equals(x.ProviderKey, profile.SystemEmailProvider, StringComparison.OrdinalIgnoreCase));

            // 6) pick provider config for THIS profile
            object providerConfig = profile.SystemEmailProvider.ToUpperInvariant() switch
            {
                "GMAIL" => GetActiveGmailConfig(profile)
                    ?? throw new InvalidOperationException("No active Gmail config found for this sender profile."),

                "MSGRAPH" => GetActiveMsGraphConfig(profile)
                    ?? throw new InvalidOperationException("No active MS Graph config found for this sender profile."),

                _ => throw new InvalidOperationException($"Unsupported provider: {profile.SystemEmailProvider}")
            };

            var ctx = new ProviderSendContext(
                ToEmail: req.ToEmail,
                Subject: subject,
                Body: body,
                IsHtml: template.IsHtml,
                ProviderConfig: providerConfig,
                FromEmail: profile.SystemEmail
            );

            await sender.SendAsync(ctx);
        }

        private Task<EmailTemplate?> GetByKeyAsync(string templateKey) =>
            _db.EmailTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TemplateKey == templateKey);


        private async Task<EmailConfigDTO?> GetProfileByKeyOrDefaultAsync()
        {
            return new EmailConfigDTO(
                SystemEmail: await _db.GlobalConfigs.AsNoTracking()
                .Where(c => c.Key == "SYSTEM_EMAIL")
                .Select(c => c.Value)
                .FirstOrDefaultAsync() ?? "",
                SystemEmailConfigKey: await _db.GlobalConfigs.AsNoTracking()
                .Where(c => c.Key == "SYSTEM_EMAIL_CONFIG_KEY")
                .Select(c => c.Value)
                .FirstOrDefaultAsync() ?? "",
                SystemEmailProvider: await _db.GlobalConfigs.AsNoTracking()
                    .Where(c => c.Key == "SYSTEM_EMAIL_PROVIDER")
                    .Select(c => c.Value)
                    .FirstOrDefaultAsync() ?? ""
            );


        }

        private GmailOauthConfig? GetActiveGmailConfig(EmailConfigDTO profile)
        {
            if (!Guid.TryParse(profile.SystemEmailConfigKey, out var configId))
                return null;

            return _db.GmailOauthConfigs
                .AsNoTracking()
                .FirstOrDefault(x => x.GmailConfigId == configId);
        }

        private MsGraphOauthConfig? GetActiveMsGraphConfig(EmailConfigDTO profile)
        {
            if (!Guid.TryParse(profile.SystemEmailConfigKey, out var configId))
                return null;
           
            return _db.MsGraphOauthConfigs
               .AsNoTracking()
               .FirstOrDefault(x => x.MsGraphConfigId == configId);
        }
    }
}
