namespace IDV.Application.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalClients { get; set; }
        public int TotalVerifications { get; set; }
        public int TotalProducts { get; set; }
        public int TodayRegistrations { get; set; }
        public double SuccessRate { get; set; }
        public double AvgResponseTime { get; set; }
        public List<ActivityLogDto> RecentActivity { get; set; } = new();
    }

    public class ActivityLogDto
    {
        public string Id { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}