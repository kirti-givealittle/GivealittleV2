using GivealittleV2.Domain.Models.Auth;

namespace GivealittleV2.Application.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegistrationDTO req, string? ip, string? userAgent);
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO req, string? ip, string? userAgent);
        Task<AuthResponseDTO> RefreshAsync(string refreshToken, string? ip, string? userAgent);
        Task LogoutAsync(string refreshToken, string? ip, string? userAgent);
    }
}
