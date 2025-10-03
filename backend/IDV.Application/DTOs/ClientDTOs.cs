using System.ComponentModel.DataAnnotations;

namespace IDV.Application.DTOs;

// Client Registration DTOs
public class RegisterClientRequestDto
{
    public Guid? ClientId { get; set; } // From ID verification
    
    [Required]
    public string IDNumber { get; set; } = string.Empty;
    
    [Required]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    public string Gender { get; set; } = string.Empty;
    
    [Required]
    public string MobileNumber { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Province { get; set; } = string.Empty;
    
    [Required]
    public string District { get; set; } = string.Empty;
    
    public string PostalCode { get; set; } = string.Empty;
    
    public string? Notes { get; set; }
    
    public List<Guid> ProductIds { get; set; } = new();
}

public class RegisterClientResponseDto
{
    public Guid RegistrationId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public ClientDetailsDto? Client { get; set; }
}

public class ClientDetailsDto
{
    public Guid RegistrationId { get; set; }
    public Guid? ClientId { get; set; }
    public string IDNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public string? Notes { get; set; }
    public UserDto RegisteredBy { get; set; } = null!;
    public List<ClientProductDto> Products { get; set; } = new();
}

public class UpdateClientDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    public string MobileNumber { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Province { get; set; } = string.Empty;
    
    [Required]
    public string District { get; set; } = string.Empty;
    
    public string PostalCode { get; set; } = string.Empty;
    
    [Required]
    public string Status { get; set; } = string.Empty;
    
    public string? Notes { get; set; }
}