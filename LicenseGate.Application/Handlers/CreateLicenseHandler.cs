using LicenseGate.Application.Command;
using LicenseGate.Domain.Entities;
using LicenseGate.Domain.Interfaces;
using MediatR;

namespace LicenseGate.Application.Handlers;

public class CreateLicenseHandler : IRequestHandler<CreateLicenseCommand, CreateLicenseResult>
{
    private readonly ILicenseRepository _licenseRepository;

    public CreateLicenseHandler(ILicenseRepository licenseRepository)
    {
        _licenseRepository = licenseRepository;
    }

    public async Task<CreateLicenseResult> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.LicenseKey))
            return CreateLicenseResult.Failure("License key is required");
        
        if (await _licenseRepository.ExistsAsync(request.LicenseKey, cancellationToken))
            return CreateLicenseResult.Failure("License key already exists");
        var license = License.Create(request.LicenseKey, request.Duration);
        
        await _licenseRepository.CreateAsync(license, cancellationToken);
        return CreateLicenseResult.Successful(license.LicenseKey);
        
    }
    
}