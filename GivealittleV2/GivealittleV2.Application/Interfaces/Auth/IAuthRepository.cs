using GivealittleV2.Domain.Models.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Interfaces.Auth
{
    public interface IAuthRepository
    {
        Task<AuthResponseDTO> LoginUserAsync(LoginRequestDTO req, string? ip, string? ua);
        Task<AuthResponseDTO> RegisterUserAsync(RegistrationDTO req, string? ip, string? ua);
        Task<AuthResponseDTO> RefreshUserAsync(string refreshToken, string? ip, string? ua);
        Task LogoutUserAsync(string refreshToken, string? ip, string? ua);
        Task<AuthResponseDTO> LoginVerifyAsync(LoginVerifyOtpDTO req, string? ip, string? ua);
    }
}
