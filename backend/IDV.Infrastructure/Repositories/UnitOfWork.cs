using Microsoft.EntityFrameworkCore.Storage;
using IDV.Core.Interfaces;
using IDV.Infrastructure.Data;

namespace IDV.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDVDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(IDVDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        IDSourceClients = new IDSourceRepository(_context);
        RegisteredClients = new RegisteredClientRepository(_context);
        Products = new ProductRepository(_context);
        ClientProducts = new ClientProductRepository(_context);
        AuditLogs = new AuditLogRepository(_context);
        VerificationAttempts = new VerificationAttemptRepository(_context);
    }

    public IUserRepository Users { get; }
    public IIDSourceRepository IDSourceClients { get; }
    public IRegisteredClientRepository RegisteredClients { get; }
    public IProductRepository Products { get; }
    public IClientProductRepository ClientProducts { get; }
    public IAuditLogRepository AuditLogs { get; }
    public IVerificationAttemptRepository VerificationAttempts { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}