namespace LicenseGate.API.Services;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(string username, string password);
    string GenerateJwtToken(string username);
    
}