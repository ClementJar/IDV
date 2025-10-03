using IDV.Core.Entities;

namespace IDV.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IIDSourceRepository IDSourceClients { get; }
    IRegisteredClientRepository RegisteredClients { get; }
    IProductRepository Products { get; }
    IClientProductRepository ClientProducts { get; }
    IAuditLogRepository AuditLogs { get; }
    IVerificationAttemptRepository VerificationAttempts { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}