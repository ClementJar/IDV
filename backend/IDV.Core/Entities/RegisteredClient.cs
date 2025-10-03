using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDV.Core.Entities;

public class RegisteredClient
{
    public Guid RegistrationId { get; set; } = Guid.NewGuid();
    
    [ForeignKey("IDSourceClient")]
    public Guid? ClientId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string IDNumber { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;
    
    public DateTime DateOfBirth { get; set; }
    
    [StringLength(20)]
    public string Gender { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string MobileNumber { get; set; } = string.Empty;
    
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string Province { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string District { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string PostalCode { get; set; } = string.Empty;
    
    [Required]
    [ForeignKey("RegisteredBy")]
    public Guid RegisteredByUserId { get; set; }
    
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Active"; // Active, Pending, Suspended
    
    public string? Notes { get; set; }
    
    // Navigation properties
    public virtual IDSourceClient? IDSourceClient { get; set; }
    public virtual User RegisteredBy { get; set; } = null!;
    public virtual ICollection<ClientProduct> ClientProducts { get; set; } = new List<ClientProduct>();
}