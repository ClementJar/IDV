using Microsoft.AspNetCore.Mvc;
using IDV.Application.DTOs;
using IDV.Application.Interfaces;

namespace IDV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { message = "Login request cannot be null" });
            }

            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Username and password are required" });
            }

            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the full exception for debugging
            var logger = HttpContext.RequestServices.GetRequiredService<ILogger<AuthController>>();
            logger.LogError(ex, "Login error for username: {Username}", request?.Username);
            
            return StatusCode(500, new { 
                message = "An internal server error occurred during login",
                error = ex.Message 
            });
        }
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            await _authService.LogoutAsync(token);
        }
        return Ok(new { message = "Logged out successfully" });
    }
}