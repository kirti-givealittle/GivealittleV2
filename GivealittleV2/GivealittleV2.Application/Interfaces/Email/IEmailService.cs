using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Interfaces.Email
{
    public interface IEmailService
    {
        Task SendAsync(
            string to,
            string subject,
            string body,
            bool isHtml = true,
            CancellationToken cancellationToken = default);

        Task<string> RenderOTPTemplate(string UserName, string OTP, int Minutes);
    }
}
