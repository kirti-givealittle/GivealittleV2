using GivealittleV2.Server.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GivealittleV2.Server.Controllers
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
        public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
            => Ok(await _auth.RegisterAsync(req, Ip, UA));

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
            => Ok(await _auth.LoginAsync(req, Ip, UA));

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh(RefreshRequest req)
            => Ok(await _auth.RefreshAsync(req.RefreshToken, Ip, UA));

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshRequest req)
        {
            await _auth.LogoutAsync(req.RefreshToken, Ip, UA);
            return NoContent();
        }
    }


    //[ApiController]
    //[Route("api/me")]
    //public class MeController : ControllerBase
    //{
    //    [HttpGet]
    //    [Authorize]
    //    public IActionResult Get() => Ok(new { message = "You are authenticated" });

    //    [HttpGet("admin")]
    //    [Authorize(Roles = "Admin")]
    //    public IActionResult Admin() => Ok(new { message = "You are admin" });
    //}
}
