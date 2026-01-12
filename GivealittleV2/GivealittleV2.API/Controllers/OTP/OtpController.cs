using GivealittleV2.Application.Interfaces.OTP;
using GivealittleV2.Domain.Models.OTP.DTOs;
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
        public async Task<ContentResult> verifyRegistrationAsync(OtpPurpose purpose, string userEmail, string otp)
        {
            try
            {
               var verificationResult =  await _otpService.ValidateAsync(userEmail, purpose, otp);
                return Content(verificationResult.HtmlContent ?? "verification failed", "text/html; charset=utf-8");
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        [HttpPost("resend")]
        public async Task<ContentResult> resendOtp(OtpLoginResendDTO otpResendDTP)
        {
            try
            {
                var verificationResult = await _otpService.CreateAndSendAsync(otpResendDTP.userEmail, otpResendDTP.userFName, OtpPurpose.Login);
                var json = System.Text.Json.JsonSerializer.Serialize(verificationResult);
                return Content(json, "application/json; charset=utf-8");
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
