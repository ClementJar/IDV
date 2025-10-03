using Microsoft.EntityFrameworkCore;
using IDV.Core.Entities;

namespace IDV.Infrastructure.Data;

public class IDVDbContext : DbContext
{
    public IDVDbContext(DbContextOptions<IDVDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<IDSourceClient> IDSourceClients { get; set; }
    public DbSet<RegisteredClient> RegisteredClients { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ClientProduct> ClientProducts { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<VerificationAttempt> VerificationAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // IDSourceClient configuration
        modelBuilder.Entity<IDSourceClient>(entity =>
        {
            entity.HasKey(e => e.ClientId);
            entity.HasIndex(e => e.IDNumber).IsUnique();
        });

        // RegisteredClient configuration
        modelBuilder.Entity<RegisteredClient>(entity =>
        {
            entity.HasKey(e => e.RegistrationId);
            entity.HasIndex(e => e.IDNumber);
            
            entity.HasOne(e => e.RegisteredBy)
                .WithMany(u => u.RegisteredClients)
                .HasForeignKey(e => e.RegisteredByUserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.IDSourceClient)
                .WithMany(c => c.RegisteredClients)
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Product configuration
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId);
            entity.HasIndex(e => e.ProductCode).IsUnique();
            entity.Property(e => e.PremiumAmount).HasPrecision(18, 2);
        });

        // ClientProduct configuration
        modelBuilder.Entity<ClientProduct>(entity =>
        {
            entity.HasKey(e => e.ClientProductId);
            entity.HasIndex(e => new { e.RegistrationId, e.ProductId }).IsUnique();
            entity.Property(e => e.PremiumAmount).HasPrecision(18, 2);
            
            entity.HasOne(e => e.RegisteredClient)
                .WithMany(c => c.ClientProducts)
                .HasForeignKey(e => e.RegistrationId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Product)
                .WithMany(p => p.ClientProducts)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // AuditLog configuration
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditId);
            entity.HasIndex(e => e.Timestamp);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // VerificationAttempt configuration
        modelBuilder.Entity<VerificationAttempt>(entity =>
        {
            entity.HasKey(e => e.AttemptId);
            entity.HasIndex(e => e.SearchTimestamp);
            entity.HasIndex(e => e.IDNumber);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.VerificationAttempts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}