using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IDV.Application.DTOs;
using IDV.Application.Interfaces;

namespace IDV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VerificationController : ControllerBase
{
    private readonly IIDVerificationService _verificationService;

    public VerificationController(IIDVerificationService verificationService)
    {
        _verificationService = verificationService;
    }

    [HttpGet("{idNumber}")]
    public async Task<ActionResult<IDVerificationResponseDto>> VerifyID(string idNumber)
    {
        // URL decode the ID number to handle special characters like forward slashes
        var decodedIdNumber = Uri.UnescapeDataString(idNumber);
        var userId = GetCurrentUserId();
        var result = await _verificationService.VerifyIDNumberAsync(decodedIdNumber, userId);
        return Ok(result);
    }

    [HttpGet("multi-source/{idNumber}")]
    public async Task<ActionResult<MultiSourceVerificationResponseDto>> VerifyIDMultiSource(string idNumber)
    {
        // URL decode the ID number to handle special characters like forward slashes
        var decodedIdNumber = Uri.UnescapeDataString(idNumber);
        var userId = GetCurrentUserId();
        var result = await _verificationService.SearchMultipleSourcesWithProgressAsync(decodedIdNumber, userId);
        return Ok(result);
    }

    [HttpGet("available-test-ids")]
    public async Task<ActionResult<List<AvailableTestIdDto>>> GetAvailableTestIds()
    {
        var result = await _verificationService.GetAvailableTestIdsAsync();
        return Ok(result);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}