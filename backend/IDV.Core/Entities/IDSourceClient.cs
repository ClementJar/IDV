using System.ComponentModel.DataAnnotations;

namespace IDV.Core.Entities;

public class IDSourceClient
{
    public Guid ClientId { get; set; } = Guid.NewGuid();
    
    [Required]
    [StringLength(50)]
    public string IDType { get; set; } = string.Empty; // NationalID, Passport, DriversLicense
    
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
    
    [StringLength(100)]
    public string Province { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string District { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string PostalCode { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Source { get; set; } = string.Empty; // INRIS, ZRA, MNO
    
    public bool IsVerified { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<RegisteredClient> RegisteredClients { get; set; } = new List<RegisteredClient>();
}