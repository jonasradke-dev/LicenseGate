namespace LicenseGate.API.Models;

public class CreateLicenseRequest
{
    public string LicenseKey { get; set; } = string.Empty;
    public string? Duration { get; set; }
}