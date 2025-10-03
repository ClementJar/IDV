using System.ComponentModel.DataAnnotations;

namespace IDV.Application.DTOs;

// Product Management DTOs
public class ProductDto
{
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PremiumAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AttachProductRequestDto
{
    [Required]
    public Guid RegistrationId { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
    
    public decimal? CustomPremiumAmount { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public string? Notes { get; set; }
}

public class ClientProductDto
{
    public Guid ClientProductId { get; set; }
    public Guid RegistrationId { get; set; }
    public Guid ProductId { get; set; }
    public ProductDto Product { get; set; } = null!;
    public DateTime EnrollmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PremiumAmount { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }
}

public class UpdateClientProductDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
    
    public decimal? PremiumAmount { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public string? Notes { get; set; }
}