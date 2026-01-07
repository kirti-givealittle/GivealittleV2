using GivealittleV2.Application.Interfaces.OTP;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GivealittleV2.API.Controllers.OTP
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            _otpService = otpService;
        }
        // GET: api/<OtpController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET 
        [HttpGet("registration/verify/{purpose}/{userEmail}/{otp}")]
        public async Task<ContentResult> verifyAsync(OtpPurpose purpose, string userEmail, string otp)
        {
            try
            {
                await _otpService.ValidateAsync(userEmail, purpose, otp);
                var html = "<!DOCTYPE html>\r\n    <html>\r\n    <head>\r\n        <meta charset=\"UTF-8\">\r\n        <title>Account Verification</title>\r\n    </head>\r\n    <body style=\"font-family: Arial, sans-serif; line-height: 1.6; color: #333;\">\r\n        <p>Hi,</p>\r\n\r\n        <p>\r\n            Verification successful!\r\n        </p>\r\n\r\n        <p style=\"font-size: 24px; font-weight: bold; letter-spacing: 3px;\">\r\n            Your account is verified with us. You can close this window now\r\n        </p>\r\n\r\n        <p style=\"font-size: 12px; color: #777;\">\r\n            Need help? Contact us at {{SupportEmail}}\r\n        </p>\r\n    </body>\r\n    </html>";
                return Content(html, "text/html; charset=utf-8");
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        // POST api/<OtpController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OtpController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OtpController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
