public sealed record OtpCreateResult(
    bool success,
    Guid OtpId,
    DateTime ExpiresAtUtc,
    DateTime? NextResendAllowedAtUtc
);