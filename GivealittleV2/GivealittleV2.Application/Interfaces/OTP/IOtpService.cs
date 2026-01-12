using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Interfaces.OTP
{
    public interface IOtpService
    {
        Task<OtpCreateResult> CreateAndSendAsync(
            string userEmail,
            string userFName,
            OtpPurpose purpose);
        Task<OtpCreateResult> GenerateAndSendTokenEmail(string email, string fName, OtpPurpose registration);
        Task<OtpValidationResult> ValidateAsync(
            string userEmail,
            OtpPurpose purpose,
            string otp);

        Task ValidateAsync(OtpPurpose purpose, string otp);
    }
}
