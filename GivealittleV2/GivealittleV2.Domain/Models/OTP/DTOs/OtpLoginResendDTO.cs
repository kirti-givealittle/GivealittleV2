using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Domain.Models.OTP.DTOs
{
    public class OtpLoginResendDTO
    {
        public string userEmail { get; set; } = string.Empty;
    }
}
