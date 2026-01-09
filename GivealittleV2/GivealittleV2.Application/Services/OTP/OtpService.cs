using GivealittleV2.Application.Interfaces.OTP;
using GivealittleV2.Domain.Models.OTP;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;

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

    public async Task<OtpCreateResult> GenerateAndSendTokenEmail(string email, string fName, OtpPurpose purpose)
    {
        var token = GenerateToken();
        var result = await _store.CreateAndSendOTPAsync(token, email, fName, purpose);
        if (result is null)
            throw new InvalidOperationException("Failed to create OTP record.");
        return result;
    }

    public static string GenerateToken(int byteLength = 32)
    {
        var bytes = RandomNumberGenerator.GetBytes(byteLength);

        // Base64Url encode (URL-safe, no + / =)
        var token = Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');

        return token; 
    }

    public Task ValidateAsync(OtpPurpose purpose, string otp)
    {
        throw new NotImplementedException();
    }
}
