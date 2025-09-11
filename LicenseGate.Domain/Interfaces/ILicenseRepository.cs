using LicenseGate.Domain.Entities;

namespace LicenseGate.Domain.Interfaces;

public interface ILicenseRepository
{
    Task<License?> GetKeyByAsync(string licenseKey, CancellationToken cancellationToken = default);
    Task<License> CreateAsync(License license, CancellationToken cancellationToken = default);
    Task UpdateAsync(License license, CancellationToken cancellationToken = default); 
    Task<bool> ExistsAsync(string licenseKey, CancellationToken cancellationToken = default);
}