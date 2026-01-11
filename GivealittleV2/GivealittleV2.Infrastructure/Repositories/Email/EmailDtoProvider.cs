using GivealittleV2.Application.Interfaces.Email;
using GivealittleV2.Domain.Models.Email.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Infrastructure.Repositories.Email
{
    public class EmailDtoProvider : IEmailDtoProvider
    {
        public IReadOnlyDictionary<string, object?> GetEmailDto( OtpEmailDto dataBag)
        {
            return new Dictionary<string, object?>
            {
                { "ToFirstName", dataBag.ToFirstName },
                { "Otp", dataBag.Otp },
                { "ExpiryMinutes", dataBag.OtpExpiryInMinutes },
                { "OtpPurpose", dataBag.OtpPurpose },
                { "SupportEmail", dataBag.SupportEmail },
                { "UserEmail", dataBag.UserEmail   }
            };
        }
    }
}
