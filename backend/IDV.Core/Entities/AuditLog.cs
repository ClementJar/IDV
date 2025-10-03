using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDV.Core.Entities;

public class AuditLog
{
    public Guid AuditId { get; set; } = Guid.NewGuid();
    
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Action { get; set; } = string.Empty; // Search, Register, Update, Export
    
    [Required]
    [StringLength(100)]
    public string EntityType { get; set; } = string.Empty;
    
    public Guid? EntityId { get; set; }
    
    public string? Details { get; set; }
    
    [StringLength(50)]
    public string? IPAddress { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}