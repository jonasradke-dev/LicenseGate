using System.Security.Cryptography;
using System.Text;
using LicenseGate.API.Models;
using LicenseGate.Application.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LicenseGate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LicenseController : ControllerBase
{
    private readonly IMediator _mediator;

    public LicenseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidateLicense([FromBody] ValidateLicenseRequest request)
    {
        var deviceFingerprint = CreateDeviceFingerprint(request.HardwareInfo);
        
        var command = new ValidateLicenseCommand(request.LicenseKey, deviceFingerprint);
        var result = await _mediator.Send(command);

        if (result.IsValid)
        {
            return Ok(new ValidateLicenseResponse
            {
                Success = true,
                Message = result.Message,
                ExpiresAt = result.ExpiresAt,
            });
        }
        
        return BadRequest(new ValidateLicenseResponse
        {
            Success = false,
            Message = result.Message
        });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateLicense([FromBody] CreateLicenseRequest request)
    {
        TimeSpan? duration = null;
        if (!string.IsNullOrEmpty(request.Duration))
        {
            duration = ParseDuration(request.Duration);
            if (duration == null)
            {
                return BadRequest(new CreateLicenseResponse
                {
                    Success = false,
                    Message = "Invalid duration format. Use formats like: 1d, 1w, 3w, 1m, 1y"
                });
            }
        }
        
        var command = new CreateLicenseCommand(request.LicenseKey, duration);
        var result = await _mediator.Send(command);

        if (result.Success)
        {
            return Ok(new CreateLicenseResponse
            {
                Success = true,
                Message = result.Message,
                LicenseKey = result.LicenseKey
            });
        }

        return BadRequest(new CreateLicenseResponse
        {
            Success = false,
            Message = result.Message
        });
    }
    
    private string CreateDeviceFingerprint(HardwareInfo hardwareInfo)
    {
        var combined = $"{hardwareInfo.CpuId}-{hardwareInfo.MotherboardSerial}-{hardwareInfo.DiskSerial}";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(combined));
        return Convert.ToHexString(hash);
    }
    
    private TimeSpan? ParseDuration(string duration)
    {
        if (string.IsNullOrWhiteSpace(duration))
            return null;
            
        var unit = duration[^1..].ToLower();
        var numberPart = duration[..^1];
        
        if (!int.TryParse(numberPart, out var number) || number <= 0)
            return null;
            
        return unit switch
        {
            "d" => TimeSpan.FromDays(number),
            "w" => TimeSpan.FromDays(number * 7),
            "m" => TimeSpan.FromDays(number * 30), 
            "y" => TimeSpan.FromDays(number * 365),
            _ => null
        };
    }
}