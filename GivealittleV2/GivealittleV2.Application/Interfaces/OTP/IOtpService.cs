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

        Task<OtpValidationResult> ValidateAsync(
            string userEmail,
            OtpPurpose purpose,
            string otp);
    }
}
