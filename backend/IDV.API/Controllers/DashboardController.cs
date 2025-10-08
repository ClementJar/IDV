using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IDV.Application.DTOs;
using IDV.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IDV.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDVDbContext _context;

        public DashboardController(IDVDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDto>> GetStats()
        {
            try
            {
                var totalClients = await _context.RegisteredClients.CountAsync();
                var totalVerifications = await _context.VerificationAttempts.CountAsync();
                var totalProducts = await _context.Products.CountAsync(p => p.IsActive);

                // Calculate today's registrations
                var today = DateTime.UtcNow.Date;
                var todayRegistrations = await _context.RegisteredClients
                    .CountAsync(c => c.RegistrationDate.Date == today);

                // Calculate success/failed counts and success rate from verification attempts
                var totalAttempts = await _context.VerificationAttempts.CountAsync();
                var successfulAttempts = await _context.VerificationAttempts
                    .CountAsync(v => v.ResultStatus == "Found");
                var failedAttempts = totalAttempts - successfulAttempts;
                var successRate = totalAttempts > 0 ? (double)successfulAttempts / totalAttempts * 100 : 0;

                // Calculate average response time
                var avgResponseTime = await _context.VerificationAttempts
                    .Where(v => v.ResponseTime > 0)
                    .AverageAsync(v => (double?)v.ResponseTime) ?? 0;

                var recentActivity = await _context.AuditLogs
                    .Include(a => a.User)
                    .OrderByDescending(a => a.Timestamp)
                    .Take(10)
                    .Select(a => new ActivityLogDto
                    {
                        Id = a.AuditId.ToString(),
                        Action = a.Action,
                        Description = a.Details ?? a.EntityType,
                        Timestamp = a.Timestamp.ToUniversalTime().ToString("O"),
                        UserId = a.UserId.ToString(),
                        UserName = a.User.FullName
                    })
                    .ToListAsync();

                var stats = new DashboardStatsDto
                {
                    TotalClients = totalClients,
                    TotalVerifications = totalVerifications,
                    TotalProducts = totalProducts,
                    SuccessfulVerifications = successfulAttempts,
                    FailedVerifications = failedAttempts,
                    RecentActivity = recentActivity,
                    TodayRegistrations = todayRegistrations,
                    SuccessRate = Math.Round(successRate, 1),
                    AvgResponseTime = Math.Round(avgResponseTime / 1000, 2) // Convert to seconds
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving dashboard stats", error = ex.Message });
            }
        }
    }
}