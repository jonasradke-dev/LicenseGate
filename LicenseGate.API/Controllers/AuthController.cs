using LicenseGate.API.Services;
using Microsoft.AspNetCore.Mvc;
using LicenseGate.API.Models;
using LoginRequest = LicenseGate.API.Models.LoginRequest;


namespace LicenseGate.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new LoginResponse
            {
                Success = false,
                Message = "Username and password are required!"
            });
        }

        var token = await _authService.AuthenticateAsync(request.Username, request.Password);
        
        if (token != null)
        {
            return Ok(new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            });
        }

        return Unauthorized(new LoginResponse
        {
            Success = false,
            Message = "Invalid credentials"
        });
        
        
    }
    
}