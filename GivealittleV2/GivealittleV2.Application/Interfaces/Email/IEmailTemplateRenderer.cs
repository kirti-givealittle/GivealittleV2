using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Interfaces.Email
{
    public interface IEmailTemplateRenderer
    {
        Task<string> RenderAsync(
            string templateName,
            IReadOnlyDictionary<string, object?> model,
            CancellationToken ct = default);
    }
}
