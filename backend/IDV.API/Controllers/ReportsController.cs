using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IDV.Application.Interfaces;
using IDV.Application.DTOs;
using System.Security.Claims;

namespace IDV.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportingService _reportingService;

    public ReportsController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    [HttpGet("clients")]
    public async Task<ActionResult<IEnumerable<ClientReportDto>>> GetClientReport([FromQuery] ExportRequestDto? filters = null)
    {
        try
        {
            var report = await _reportingService.GenerateClientReportAsync(filters);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while generating the client report", error = ex.Message });
        }
    }

    [HttpPost("export/excel")]
    public async Task<ActionResult<ExportResponseDto>> ExportToExcel([FromBody] ExportRequestDto request)
    {
        try
        {
            var result = await _reportingService.ExportToExcelAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while exporting to Excel", error = ex.Message });
        }
    }

    [HttpPost("export/pdf")]
    public async Task<ActionResult<ExportResponseDto>> ExportToPdf([FromBody] ExportRequestDto request)
    {
        try
        {
            var result = await _reportingService.ExportToPdfAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while exporting to PDF", error = ex.Message });
        }
    }

    [HttpGet("dashboard-statistics")]
    public async Task<ActionResult<DashboardStatisticsDto>> GetDashboardStatistics()
    {
        try
        {
            var statistics = await _reportingService.GetDashboardStatisticsAsync();
            return Ok(statistics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving dashboard statistics", error = ex.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}