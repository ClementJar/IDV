using System.ComponentModel.DataAnnotations;

namespace IDV.Core.Entities;

public class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Role { get; set; } = string.Empty; // Admin, Agent, Viewer
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation properties
    public virtual ICollection<RegisteredClient> RegisteredClients { get; set; } = new List<RegisteredClient>();
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    public virtual ICollection<VerificationAttempt> VerificationAttempts { get; set; } = new List<VerificationAttempt>();
}