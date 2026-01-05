using GivealittleV2.Domain.Models.OTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Interfaces.OTP
{
    public interface IOtpRepository
    {
        Task<OtpCreateResult> CreateAndSendOTPAsync(string Otp,string userEmail, string userFName, OtpPurpose purpose);
        Task<OtpValidationResult> ValidateOTPAsync(string userKey, OtpPurpose purpose, string otp);
    }
}
