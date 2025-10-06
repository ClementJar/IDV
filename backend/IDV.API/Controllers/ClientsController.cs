using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IDV.Application.DTOs;
using IDV.Application.Interfaces;

namespace IDV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IClientRegistrationService _clientService;

    public ClientsController(IClientRegistrationService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ClientRegistrationWithEposDto>> RegisterClient([FromBody] RegisterClientRequestDto request)
    {
        var userId = GetCurrentUserId();
        var result = await _clientService.RegisterNewClientAsync(request, userId);
        
        if (!result.Success)
            return BadRequest(result);
            
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDetailsDto>>> GetAllClients()
    {
        var clients = await _clientService.GetAllRegisteredClientsAsync();
        return Ok(clients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDetailsDto>> GetClient(Guid id)
    {
        var client = await _clientService.GetClientDetailsAsync(id);
        if (client == null)
            return NotFound();
            
        return Ok(client);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClientDetailsDto>> UpdateClient(Guid id, [FromBody] UpdateClientDto request)
    {
        try
        {
            var result = await _clientService.UpdateClientInfoAsync(id, request);
            return Ok(result);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteClient(Guid id)
    {
        var success = await _clientService.DeleteClientAsync(id);
        if (!success)
            return NotFound();
            
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ClientDetailsDto>>> SearchClients([FromQuery] string q)
    {
        var clients = await _clientService.SearchRegisteredClientsAsync(q);
        return Ok(clients);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
}