using GivealittleV2.Application.Interfaces.Email;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Services.Email
{
    public sealed class FileEmailTemplateRenderer : IEmailTemplateRenderer
    {
        private readonly string _rootPath;

        public FileEmailTemplateRenderer()
        {
            _rootPath = "C:\\Visal Projects\\GiveALittleV2\\GivealittleV2\\GivealittleV2\\GivealittleV2.Domain\\EmailTemplates";
        }

        public async Task<string> RenderAsync(
            string templateName,
            IReadOnlyDictionary<string, object?> model,
            CancellationToken ct = default)
        {
            // e.g. templateName = "Otp" => "Otp.html"
            var path = Path.Combine(_rootPath, $"{templateName}.html");

            if (!File.Exists(path))
                throw new FileNotFoundException($"Email template not found: {path}");

            var html = await File.ReadAllTextAsync(path, Encoding.UTF8, ct);

            // Replace tokens: {{Key}}
            foreach (var kv in model)
            {
                var token = "{{" + kv.Key + "}}";
                var value = ConvertToString(kv.Value);
                html = html.Replace(token, value, StringComparison.Ordinal);
            }

            return html;
        }

        private static string ConvertToString(object? value)
        {
            if (value is null) return string.Empty;

            return value switch
            {
                DateTime dt => dt.ToString("u", CultureInfo.InvariantCulture),
                DateTimeOffset dto => dto.ToString("u", CultureInfo.InvariantCulture),
                IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                _ => value.ToString() ?? string.Empty
            };
        }
    }
}
