public record AuthResponseDTO(
    string AccessToken, 
    DateTime AccessTokenExpiresAtUtc, 
    string RefreshToken,
    string FName,
    string LName,
    List<string> Emails,
    bool IsExistingUser,
    Guid authUserId
    );