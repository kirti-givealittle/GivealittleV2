namespace GivealittleV2.Server.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest req, string? ip, string? userAgent);
        Task<AuthResponse> LoginAsync(LoginRequest req, string? ip, string? userAgent);
        Task<AuthResponse> RefreshAsync(string refreshToken, string? ip, string? userAgent);
        Task LogoutAsync(string refreshToken, string? ip, string? userAgent);
    }
}
