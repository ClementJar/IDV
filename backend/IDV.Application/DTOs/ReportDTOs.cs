using System.ComponentModel.DataAnnotations;

namespace IDV.Application.DTOs;

// Reporting DTOs
public class ClientReportDto
{
    public Guid RegistrationId { get; set; }
    public string IDNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public int ProductCount { get; set; }
    public decimal TotalPremium { get; set; }
    public string RegisteredBy { get; set; } = string.Empty;
}

public class ExportRequestDto
{
    public string Format { get; set; } = "Excel"; // Excel, PDF
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Province { get; set; }
    public string? Status { get; set; }
    public string? Category { get; set; }
    public List<Guid>? ClientIds { get; set; }
}

public class ExportResponseDto
{
    public bool Success { get; set; }
    public string? FileName { get; set; }
    public byte[]? FileData { get; set; }
    public string? ContentType { get; set; }
    public string? ErrorMessage { get; set; }
}

public class DashboardStatisticsDto
{
    public int TotalClients { get; set; }
    public int ActivePolicies { get; set; }
    public decimal TotalRevenue { get; set; }
    public int NewRegistrationsThisMonth { get; set; }
    public List<ProvinceStatDto> ProvinceStats { get; set; } = new();
    public List<ProductCategoryStatDto> CategoryStats { get; set; } = new();
    public List<RegistrationTrendDto> RegistrationTrends { get; set; } = new();
}

public class ProvinceStatDto
{
    public string Province { get; set; } = string.Empty;
    public int ClientCount { get; set; }
    public decimal TotalPremium { get; set; }
}

public class ProductCategoryStatDto
{
    public string Category { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public int EnrollmentCount { get; set; }
    public decimal TotalPremium { get; set; }
}

public class RegistrationTrendDto
{
    public DateTime Date { get; set; }
    public int RegistrationCount { get; set; }
    public decimal Revenue { get; set; }
}