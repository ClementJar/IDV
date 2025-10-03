using System.ComponentModel.DataAnnotations;

namespace IDV.Application.DTOs;

// ID Verification DTOs
public class IDVerificationRequestDto
{
    [Required]
    public string IDNumber { get; set; } = string.Empty;
    
    public string? IDType { get; set; }
}

public class IDVerificationResponseDto
{
    public bool Success { get; set; }
    public string Status { get; set; } = string.Empty; // Found, NotFound, Multiple, Error
    public int ResultCount { get; set; }
    public int ResponseTime { get; set; }
    public string Source { get; set; } = string.Empty;
    public List<ClientSearchResultDto> Results { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class ClientSearchResultDto
{
    public Guid ClientId { get; set; }
    public string IDType { get; set; } = string.Empty;
    public string IDNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
}

public class MultipleRecordsResponseDto
{
    public string IDNumber { get; set; } = string.Empty;
    public int TotalFound { get; set; }
    public List<ClientSearchResultDto> Records { get; set; } = new();
    public string Message { get; set; } = string.Empty;
}

// Multi-source verification DTOs
public class MultiSourceVerificationResponseDto
{
    public bool Success { get; set; }
    public string IDNumber { get; set; } = string.Empty;
    public List<SourceSearchResultDto> SourceResults { get; set; } = new();
    public ClientSearchResultDto? FinalResult { get; set; }
    public int TotalResponseTime { get; set; }
    public string OverallStatus { get; set; } = string.Empty;
}

public class SourceSearchResultDto
{
    public string SourceName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Checking, Found, NotFound, Error, Timeout
    public int ResponseTime { get; set; }
    public bool IsFound { get; set; }
    public ClientSearchResultDto? Result { get; set; }
    public string? ErrorMessage { get; set; }
    public int Priority { get; set; } // Search priority order
}

public class AvailableTestIdDto
{
    public string IdNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string DisplaySource { get; set; } = string.Empty;
}