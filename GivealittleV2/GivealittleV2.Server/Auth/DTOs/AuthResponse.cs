public record AuthResponse(
    string AccessToken, 
    DateTime AccessTokenExpiresAtUtc, 
    string RefreshToken
    );