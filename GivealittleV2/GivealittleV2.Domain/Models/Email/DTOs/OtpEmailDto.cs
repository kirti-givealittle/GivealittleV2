public sealed record OtpEmailDto(
    string Otp,
    string ToFirstName,
    string OtpPurpose,
    int OtpExpiryInMinutes,
    string SupportEmail,
    string UserEmail
);