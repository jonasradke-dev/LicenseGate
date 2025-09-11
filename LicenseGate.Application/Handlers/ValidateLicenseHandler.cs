using LicenseGate.Application.Command;
using LicenseGate.Domain.Interfaces;
using LicenseGate.Domain.ValueObjects;
using MediatR;

namespace LicenseGate.Application.Handlers;

public class ValidateLicenseHandler : IRequestHandler<ValidateLicenseCommand, LicenseValidationResult>
{
    private readonly ILicenseRepository _licenseRepository;

    public ValidateLicenseHandler(ILicenseRepository licenseRepository)
    {
        _licenseRepository = licenseRepository;
    }

    public async Task<LicenseValidationResult> Handle(ValidateLicenseCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.LicenseKey))
            return LicenseValidationResult.Failure("License key is required");
        
        if (string.IsNullOrWhiteSpace(request.DeviceFingerprint))
            return LicenseValidationResult.Failure("Device fingerprint is required");

        var license = await _licenseRepository.GetKeyByAsync(request.LicenseKey, cancellationToken);
        if(license is null)
            return LicenseValidationResult.Failure("License not found");
        
        var result = license.ValidateAndActivate(request.DeviceFingerprint);

        if (result.IsValid || license.Status == LicenseStatus.Expired)
        {
            await _licenseRepository.UpdateAsync(license, cancellationToken);
        }
        return result;
    }
    
}