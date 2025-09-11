using LicenseGate.Domain.Entities;
using LicenseGate.Domain.Interfaces;
using LicenseGate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LicenseGate.Infrastructure.Repositories;

public class LicenseRepository : ILicenseRepository
{
    private readonly LicenseDbContext _context;

    public LicenseRepository(LicenseDbContext context)
    {
        _context = context;
    }


    public async Task<License?> GetKeyByAsync(string licenseKey, CancellationToken cancellationToken = default)
    {
        return await _context.Licenses
            .FirstOrDefaultAsync(l=>l.LicenseKey == licenseKey, cancellationToken);
    }

    public async Task<License> CreateAsync(License license, CancellationToken cancellationToken = default)
    {
        _context.Licenses.Add(license);
        await _context.SaveChangesAsync(cancellationToken);
        return license;
    }

    public async Task UpdateAsync(License license, CancellationToken cancellationToken = default)
    {
        _context.Licenses.Update(license);
        await _context.SaveChangesAsync(cancellationToken);

    }
    
    public async Task<bool> ExistsAsync(string licenseKey, CancellationToken cancellationToken = default)
    {
        return await _context.Licenses.
            AnyAsync(l => l.LicenseKey == licenseKey, cancellationToken);
    }
    
}