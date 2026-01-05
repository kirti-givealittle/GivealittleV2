using GivealittleV2.Application.Interfaces.Auth;
using GivealittleV2.Domain.Models.Auth.DTOs;

namespace GivealittleV2.Application.Services.Auth
{
    public sealed class AuthService: IAuthService
    {
        private readonly IAuthRepository authRepository;
        

        public AuthService(IAuthRepository _authRepository)
        {
            authRepository = _authRepository;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegistrationDTO req, string? ip, string? ua)
        {
            return await authRepository.RegisterUserAsync(req, ip, ua);
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO req, string? ip, string? ua)
        {
            return await authRepository.LoginUserAsync(req, ip, ua);
        }

        public async Task<AuthResponseDTO> RefreshAsync(string refreshToken, string? ip, string? ua)
        {
            return await authRepository.RefreshUserAsync(refreshToken, ip, ua);
        }

        public async Task LogoutAsync(string refreshToken, string? ip, string? ua)
        {
            await authRepository.LogoutUserAsync(refreshToken, ip, ua);
        }

        

        
    }
}
 