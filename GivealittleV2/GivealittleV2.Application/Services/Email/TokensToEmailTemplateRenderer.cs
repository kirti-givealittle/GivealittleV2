using GivealittleV2.Application.Interfaces.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Services.Email
{
    public class TokensToEmailTemplateRenderer : IEmailTemplateRenderer
    {
        // tokens like {{Key}}
        private static readonly Regex TokenRegex = new(@"\{\{\s*(?<key>[\w\.\-]+)\s*\}\}",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public string Render(string template, IReadOnlyDictionary<string, object?> model)
        {
            if (string.IsNullOrEmpty(template)) return template;

            return TokenRegex.Replace(template, m =>
            {
                var key = m.Groups["key"].Value;

                if (!model.TryGetValue(key, out var value) || value is null)
                    return string.Empty;

                return Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty;
            });
        }
    }
}
