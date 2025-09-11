namespace LicenseGate.API.Models;

public class CreateLicenseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? LicenseKey { get; set; }
}