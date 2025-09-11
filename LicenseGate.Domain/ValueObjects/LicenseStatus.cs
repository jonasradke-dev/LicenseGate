namespace LicenseGate.Domain.ValueObjects;

public enum LicenseStatus
{
    Unused = 0,
    Active = 1,
    Revoked = 2,
    Suspended = 3,
    Expired = 4,
}