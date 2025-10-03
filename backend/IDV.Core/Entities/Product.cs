using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDV.Core.Entities;

public class Product
{
    public Guid ProductId { get; set; } = Guid.NewGuid();
    
    [Required]
    [StringLength(50)]
    public string ProductCode { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string ProductName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty; // Life, Health, Savings, Pension, Asset
    
    public string? Description { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal PremiumAmount { get; set; }
    
    [Required]
    [StringLength(10)]
    public string Currency { get; set; } = "ZMW";
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual ICollection<ClientProduct> ClientProducts { get; set; } = new List<ClientProduct>();
}