using GivealittleV2.Application.Interfaces.Email;
using GivealittleV2.Infrastructure.Persistence.Models;
using Google.Apis.Auth.OAuth2;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Infrastructure.Repositories.Email
{
    public class GmailApiEmailProviderSender : IEmailProviderSender
    {
        public string ProviderKey => "GMAIL";

        public async Task SendAsync(ProviderSendContext ctx)
        {
            if (ctx.ProviderConfig is not GmailOauthConfig cfg)
                throw new InvalidOperationException("Invalid provider config type for Gmail.");

            // 1) Get a valid access token (refresh if needed)
            var accessToken = await GetValidAccessTokenAsync(cfg);

            // 2) Build email
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(ctx.FromEmail));
            message.To.Add(MailboxAddress.Parse(ctx.ToEmail));
            message.Subject = ctx.Subject;

            var body = new BodyBuilder
            {
                HtmlBody = ctx.IsHtml ? ctx.Body : null,
                TextBody = ctx.IsHtml ? null : ctx.Body
            };
            message.Body = body.ToMessageBody();

            // 3) Send via Gmail SMTP with OAuth2 (XOAUTH2)
            using var smtp = new MailKit.Net.Smtp.SmtpClient(); // Use MailKit's SmtpClient

            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            // Replace this line:
            // var oauth2 = new SaslMechanismOAuth2(cfg.GmailUserEmail, accessToken);

            // With this line:
            var oauth2 = new SaslMechanismOAuth2(ctx.FromEmail, accessToken);
            await smtp.AuthenticateAsync(oauth2);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

        private static async Task<string> GetValidAccessTokenAsync(GmailOauthConfig cfg)
        {
            // if token exists and not expiring soon, reuse it
            if (!string.IsNullOrWhiteSpace(cfg.AccessToken) &&
                cfg.AccessTokenExpiresAtUtc.HasValue &&
                cfg.AccessTokenExpiresAtUtc.Value > DateTime.UtcNow.AddMinutes(2))
            {
                return cfg.AccessToken!;
            }

            // Refresh using GoogleCredential
            var credential = GoogleCredential.FromAccessToken("gggg");


            var token = await RefreshAccessTokenAsync(
                cfg.ClientId,
                cfg.ClientSecret,
                cfg.RefreshToken);

            cfg.AccessToken = token.AccessToken;
            cfg.AccessTokenExpiresAtUtc = DateTime.UtcNow.AddSeconds(token.ExpiresIn);


            return cfg.AccessToken!;
        }

        private sealed record TokenResponse(string AccessToken, int ExpiresIn);

        private static async Task<TokenResponse> RefreshAccessTokenAsync(
            string clientId,
            string clientSecret,
            string refreshToken)
        {
            using var http = new HttpClient();

            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["refresh_token"] = refreshToken,
                ["grant_type"] = "refresh_token"
            });

            using var resp = await http.PostAsync("https://oauth2.googleapis.com/token", form);
            var json = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException($"Failed to refresh Gmail access token. Status={resp.StatusCode}. Body={json}");

            // minimal JSON parse without extra packages
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var accessToken = doc.RootElement.GetProperty("access_token").GetString()!;
            var expiresIn = doc.RootElement.GetProperty("expires_in").GetInt32();

            return new TokenResponse(accessToken, expiresIn);
        }
    }
}
