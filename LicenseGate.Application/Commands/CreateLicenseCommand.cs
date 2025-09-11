using MediatR;

namespace LicenseGate.Application.Command;

public record CreateLicenseCommand(
    string LicenseKey,
    TimeSpan? Duration = null
    ) : IRequest<CreateLicenseResult>;

public record CreateLicenseResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? LicenseKey { get; init; }

    public static CreateLicenseResult Successful(string licenseKey)
        => new() {Success = true, Message = "License created successfully", LicenseKey = licenseKey};
    public static CreateLicenseResult Failure(string message)
        => new() {Success = false, Message = message};
}