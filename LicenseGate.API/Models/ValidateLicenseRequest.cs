namespace LicenseGate.API.Models;

public class ValidateLicenseRequest
{
    public string LicenseKey { get; set; } = string.Empty;
    public HardwareInfo HardwareInfo { get; set; } = new();
}

public class HardwareInfo
{
    public string CpuId { get; set; } = string.Empty;
    public string MotherboardSerial { get; set; } = string.Empty;
    public string DiskSerial { get; set; } = string.Empty;
}