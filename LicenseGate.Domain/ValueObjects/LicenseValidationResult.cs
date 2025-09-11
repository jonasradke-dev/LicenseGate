namespace LicenseGate.Domain.ValueObjects;

public class LicenseValidationResult
{
    public bool IsValid { get; private set; }
    public string message { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    private LicenseValidationResult(bool isValid, string message, DateTime? expiresAt = null)
    {
        IsValid = isValid;
        this.message = message;
        ExpiresAt = expiresAt;
    }
    
    public static LicenseValidationResult Success(string message, DateTime? expiresAt = null)
        => new(true, message, expiresAt);
    
    public static LicenseValidationResult Failure(string message)
        =>new(false, message);
    
    
    
}