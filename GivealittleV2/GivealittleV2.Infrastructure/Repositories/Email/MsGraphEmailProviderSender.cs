using GivealittleV2.Application.Interfaces.Email;
using GivealittleV2.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GivealittleV2.Infrastructure.Repositories.Email
{
    public sealed class MsGraphEmailProviderSender : IEmailProviderSender
    {
        public string ProviderKey => "MSGRAPH";

        private readonly HttpClient _http;

        public MsGraphEmailProviderSender(HttpClient http) => _http = http;

        public async Task SendAsync(ProviderSendContext ctx)
        {
            var cfg = (MsGraphOauthConfig)ctx.ProviderConfig;

            
            var token = await GetAccessTokenAsync(cfg);

            // Send mail endpoint: /users/{from}/sendMail
            var url = $"https://graph.microsoft.com/v1.0/users/{Uri.EscapeDataString(ctx.FromEmail)}/sendMail";

            var payload = new
            {
                message = new
                {
                    subject = ctx.Subject,
                    body = new
                    {
                        contentType = ctx.IsHtml ? "HTML" : "Text",
                        content = ctx.Body
                    },
                    toRecipients = new[]
                    {
                    new { emailAddress = new { address = ctx.ToEmail } }
                }
                },
                saveToSentItems = "true"
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var res = await _http.SendAsync(req);

            if (!res.IsSuccessStatusCode)
            {
                var body = await res.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Graph sendMail failed: {(int)res.StatusCode} {res.ReasonPhrase}. Body: {body}");
            }
        }

        private static async Task<string> GetAccessTokenAsync(MsGraphOauthConfig cfg)
        {
            using var http = new HttpClient();

            var tokenUrl = $"https://login.microsoftonline.com/{cfg.TenantId}/oauth2/v2.0/token";

            var form = new Dictionary<string, string>
            {
                ["client_id"] = cfg.ClientId,
                ["client_secret"] = cfg.ClientSecret,
                ["grant_type"] = "client_credentials"
                //["scope"] = cfg.Scope
            };

            using var res = await http.PostAsync(tokenUrl, new FormUrlEncodedContent(form));

            var json = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                throw new InvalidOperationException($"Token request failed: {(int)res.StatusCode} {res.ReasonPhrase}. Body: {json}");

            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString()
                   ?? throw new InvalidOperationException("access_token missing in token response.");
        }
    }
}
