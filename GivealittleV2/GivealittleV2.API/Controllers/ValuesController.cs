using GivealittleV2.Application.Interfaces.OTP;
using GivealittleV2.Domain.Models.OTP;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GivealittleV2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IOtpService otpService;
        public ValuesController(IOtpService _otpService)
        {
            otpService = _otpService;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost("register")]
        public async Task PostAsync([FromBody] OtpGenerationRequestDTO OtpRegistrationDTO)
        {
            await otpService.CreateAndSendAsync(OtpRegistrationDTO.userEmail, OtpRegistrationDTO.userFName, OtpRegistrationDTO.Purpose);
        }

        [HttpPost("validate")]
        public async Task PostAsync([FromBody] OtpValidationRequestDTO otpValidationDTO)
        {
            await otpService.ValidateAsync(otpValidationDTO.userEmail, otpValidationDTO.Purpose, otpValidationDTO.OTP);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
