using GivealittleV2.Application.Interfaces.Auth;
using GivealittleV2.Domain.Models.Auth.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GivealittleV2.API.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;
        private string? Ip => HttpContext.Connection.RemoteIpAddress?.ToString();
        private string? UA => Request.Headers.UserAgent.ToString();

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegistrationDTO req)
        {
            try
            {
                return Ok(await _auth.RegisterAsync(req, Ip, UA));
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginRequestDTO req)
            => Ok(await _auth.LoginAsync(req, Ip, UA));

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDTO>> Refresh(RefreshRequestDTO req)
            => Ok(await _auth.RefreshAsync(req.RefreshToken, Ip, UA));

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshRequestDTO req)
        {
            await _auth.LogoutAsync(req.RefreshToken, Ip, UA);
            return NoContent();
        }
    }
}
