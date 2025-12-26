public sealed record OtpValidationResult(
    bool IsValid,
    string? FailureReason = null
);