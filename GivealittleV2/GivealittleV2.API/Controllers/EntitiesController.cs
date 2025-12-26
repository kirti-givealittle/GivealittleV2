using GivealittleV2.Domain.Models.Auth;
using GivealittleV2.Domain.Models.Cause;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GivealittleV2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntitiesController : ControllerBase
    {
        public EntitiesController()
        {
            
        }
        //[HttpPost("draft")]
        //public async Task<ActionResult<CauseResponse>> Register(RegistrationDTO req)
        //    => Ok(await _auth.RegisterAsync(req, Ip, UA));
    }
}
