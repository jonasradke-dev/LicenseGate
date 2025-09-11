namespace LicenseGate.API.Models;

public class ValidateLicenseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
}
