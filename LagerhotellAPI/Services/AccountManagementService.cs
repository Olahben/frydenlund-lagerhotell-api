using LagerhotellAPI.Models;
using LagerhotellAPI.Models.DbModels;
using LagerhotellAPI.Models.CustomExceptionModels;

namespace LagerhotellAPI.Services;

public class AccountManagementService
{
    private readonly MongoDbSettings _settings;
    private readonly IMongoCollection<EmailVerificationCodeDocument> _verificationCodes;
    public AccountManagementService(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase("Lagerhotell");
        _verificationCodes = database.GetCollection<EmailVerificationCodeDocument>("EmailVerificationCodes");
    }
    public int CreateVerifyEmailCode()
    {
        int code = new Random().Next(1000, 9999);
        return code;
    }

    public async Task<int> GetMostRecentCode(string email)
    {
        EmailVerificationCodeDocument? emailVerificationCodeDocument = await _verificationCodes.Find(document => document.Email == email).SortByDescending(document => document.TimeCreated).FirstOrDefaultAsync();
        if (emailVerificationCodeDocument == null)
        {
            throw new KeyNotFoundException("No email verification code found");
        }
        if (emailVerificationCodeDocument.TimeCreated.AddMinutes(15) < DateTime.Now)
        {
            throw new InvalidOperationException("Email verification code has expired");
        }
        return emailVerificationCodeDocument.Code;
    }

    public async Task<bool> VerifyEmailVerificationCode(string email, int code)
    {
        int mostRecentCode;
        try
        {
            mostRecentCode = await GetMostRecentCode(email);
        } catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException("No email verification code found");

        } catch (InvalidOperationException)
        {
            throw new InvalidOperationException("Email verification code has expired");
        }
        if (mostRecentCode == code)
        {
            return true;
        }
        throw new InvalidVerificationCodeException("Invalid verification code");
    }

    public async Task StartEmailVerification(string email)
    {
        int code = CreateVerifyEmailCode();
        EmailVerificationCodeDocument document = new(code, email, DateTime.Now);
        await _verificationCodes.InsertOneAsync(document);
        // Send email with code
    }
}