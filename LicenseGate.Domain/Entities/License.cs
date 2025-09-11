using System.Data;
using LicenseGate.Domain.ValueObjects;

namespace LicenseGate.Domain.Entities;

public class License
{
    public int Id { get; set; }
    public string LicenseKey { get; set; }
    public string? DeviceFingerprint { get; set; }
    public LicenseStatus Status { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastHeartbeat { get; set; }
    public TimeSpan? Duration { get; set; }

    private License(){}

    public static License Create(string licenseKey, TimeSpan? duration = null)
    {
        if(string.IsNullOrWhiteSpace(licenseKey))
            throw new ArgumentException("License key cannot be null or empty",nameof(licenseKey));

        return new License()
        {
            LicenseKey = licenseKey,
            Status = LicenseStatus.Unused,
            CreatedAt = DateTime.UtcNow,
            Duration = duration,
            ExpiresAt = null
        };
    }

    public LicenseValidationResult ValidateAndActivate(string deviceFingerprint)
    {
        if (Status == LicenseStatus.Active && DeviceFingerprint != deviceFingerprint)
        {
            return LicenseValidationResult.Failure("HWID does not match!");
        }

        if (Status == LicenseStatus.Suspended)
        {
            return LicenseValidationResult.Failure("Your License is currently deactivated!");
        }

        if (Status == LicenseStatus.Revoked)
        {
            return LicenseValidationResult.Failure("Your License has been permanently revoked!");
        }

        if (ExpiresAt.HasValue && DateTime.Now > ExpiresAt.Value)
        {
           
            if (Status != LicenseStatus.Expired)
            {
                Status = LicenseStatus.Expired;
            }
            return LicenseValidationResult.Failure("Your License is expired!");
        }

        if (Status == LicenseStatus.Expired)
        {
            return LicenseValidationResult.Failure("Your License is expired!");
        }

        if (Status == LicenseStatus.Unused)
        {
            DeviceFingerprint = deviceFingerprint;
            Status = LicenseStatus.Active;
            ActivatedAt = DateTime.UtcNow;
            
            if (Duration.HasValue)
            {
                ExpiresAt = ActivatedAt.Value.Add(Duration.Value);
            }
            
            return LicenseValidationResult.Success("License has been activated!", ExpiresAt);
        }

        if (Status == LicenseStatus.Active && DeviceFingerprint == deviceFingerprint)
        {
            UpdateHeartbeat();
            return LicenseValidationResult.Success("Your License is valid!", ExpiresAt);
        }
        return LicenseValidationResult.Failure("Unknown validation error");
    }
    
    public void UpdateHeartbeat()
    {
        LastHeartbeat = DateTime.UtcNow;
    }

    public void Revoke()
    {
        Status = LicenseStatus.Revoked;
    }

    public void Suspend()
    {
        Status = LicenseStatus.Suspended;
    }

    public void Reactivate()
    {
        if (Status == LicenseStatus.Suspended)
            Status = LicenseStatus.Active;
    }

}




