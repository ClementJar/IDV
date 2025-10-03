using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDV.Core.Entities;

public class ClientProduct
{
    public Guid ClientProductId { get; set; } = Guid.NewGuid();
    
    [Required]
    [ForeignKey("RegisteredClient")]
    public Guid RegistrationId { get; set; }
    
    [Required]
    [ForeignKey("Product")]
    public Guid ProductId { get; set; }
    
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Active"; // Active, Lapsed, Cancelled
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal PremiumAmount { get; set; }
    
    [StringLength(100)]
    public string PolicyNumber { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public string? Notes { get; set; }
    
    // Navigation properties
    public virtual RegisteredClient RegisteredClient { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}