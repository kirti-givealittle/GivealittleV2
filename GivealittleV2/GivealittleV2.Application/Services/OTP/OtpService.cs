using GivealittleV2.Application.Interfaces.OTP;
using GivealittleV2.Domain.Models.OTP;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

public sealed class OtpService : IOtpService
{
    private readonly IOtpRepository _store;
    private readonly OtpOptions _options;

    public OtpService(IOtpRepository store, IOptions<OtpOptions> options)
    {
        _store = store;
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.Pepper))
            throw new InvalidOperationException("OtpOptions.Pepper must be configured.");
    }

    public async Task<OtpCreateResult> CreateAndSendAsync(string userEmail, string userFName, OtpPurpose purpose)
    {
        string otp = Generate6DigitOtp();
        var result = await _store.CreateAndSendOTPAsync(otp, userEmail, userFName, purpose);
        if (result is null)
            throw new InvalidOperationException("Failed to create OTP record.");
        return result;
    }

    public async Task<OtpValidationResult> ValidateAsync(string userEmail, OtpPurpose purpose, string otp)
    {
        return await _store.ValidateOTPAsync(userEmail, purpose, otp);
    }

    private static string Generate6DigitOtp()
    {
        int value = RandomNumberGenerator.GetInt32(0, 1_000_000);
        return value.ToString("D6");
    }

    
}
