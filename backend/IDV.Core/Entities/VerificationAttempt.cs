using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDV.Core.Entities;

public class VerificationAttempt
{
    public Guid AttemptId { get; set; } = Guid.NewGuid();
    
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string IDNumber { get; set; } = string.Empty;
    
    public DateTime SearchTimestamp { get; set; } = DateTime.UtcNow;
    
    [Required]
    [StringLength(50)]
    public string ResultStatus { get; set; } = string.Empty; // Found, NotFound, Multiple, Error
    
    public int ResultCount { get; set; }
    
    public int ResponseTime { get; set; } // milliseconds
    
    [StringLength(100)]
    public string SourceSystem { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}