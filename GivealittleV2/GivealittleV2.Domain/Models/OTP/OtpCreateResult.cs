public sealed record OtpCreateResult(
    Guid OtpId,
    DateTime ExpiresAtUtc,
    DateTime? NextResendAllowedAtUtc
);