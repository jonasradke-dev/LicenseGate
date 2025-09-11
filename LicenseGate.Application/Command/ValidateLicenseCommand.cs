using LicenseGate.Domain.ValueObjects;
using MediatR;

namespace LicenseGate.Application.Command;

public record ValidateLicenseCommand(
    string LicenseKey, 
    string DeviceFingerprint
    ) : IRequest<LicenseValidationResult>;
    
    