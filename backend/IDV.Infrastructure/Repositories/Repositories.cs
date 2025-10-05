using Microsoft.EntityFrameworkCore;
using IDV.Core.Entities;
using IDV.Core.Interfaces;
using IDV.Infrastructure.Data;

namespace IDV.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(IDVDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> ValidatePasswordAsync(string username, string password)
    {
        var user = await GetByUsernameAsync(username);
        if (user == null) return false;
        
        // In a real application, you would use proper password hashing
        // For demo purposes, we'll use simple comparison
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}

public class IDSourceRepository : Repository<IDSourceClient>, IIDSourceRepository
{
    public IDSourceRepository(IDVDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<IDSourceClient>> SearchByIDNumberAsync(string idNumber)
    {
        return await _dbSet
            .Where(c => c.IDNumber.Contains(idNumber))
            .ToListAsync();
    }

    public async Task<IDSourceClient?> GetByIDNumberAsync(string idNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.IDNumber == idNumber);
    }

    public async Task<IEnumerable<IDSourceClient>> GetByProvinceAsync(string province)
    {
        return await _dbSet
            .Where(c => c.Province.ToLower() == province.ToLower())
            .ToListAsync();
    }

    public async Task<IDSourceClient?> GetByIDAndSourceAsync(string idNumber, string source)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.IDNumber == idNumber && c.Source == source);
    }
}

public class RegisteredClientRepository : Repository<RegisteredClient>, IRegisteredClientRepository
{
    public RegisteredClientRepository(IDVDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<RegisteredClient>> GetAllAsync()
    {
        return await _dbSet
            .Include(c => c.RegisteredBy)
            .Include(c => c.ClientProducts)
                .ThenInclude(cp => cp.Product)
            .Include(c => c.IDSourceClient)
            .ToListAsync();
    }

    public override async Task<RegisteredClient?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.RegisteredBy)
            .Include(c => c.ClientProducts)
                .ThenInclude(cp => cp.Product)
            .Include(c => c.IDSourceClient)
            .FirstOrDefaultAsync(c => c.RegistrationId == id);
    }

    public async Task<IEnumerable<RegisteredClient>> SearchClientsAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return await _dbSet
            .Include(c => c.RegisteredBy)
            .Include(c => c.ClientProducts)
                .ThenInclude(cp => cp.Product)
            .Include(c => c.IDSourceClient)
            .Where(c => c.FullName.ToLower().Contains(lowerSearchTerm) ||
                       c.IDNumber.Contains(lowerSearchTerm) ||
                       c.Email.ToLower().Contains(lowerSearchTerm))
            .ToListAsync();
    }

    public async Task<IEnumerable<RegisteredClient>> GetByRegisteredByAsync(Guid userId)
    {
        return await _dbSet
            .Include(c => c.RegisteredBy)
            .Include(c => c.ClientProducts)
                .ThenInclude(cp => cp.Product)
            .Include(c => c.IDSourceClient)
            .Where(c => c.RegisteredByUserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<RegisteredClient>> GetByStatusAsync(string status)
    {
        return await _dbSet
            .Include(c => c.RegisteredBy)
            .Where(c => c.Status.ToLower() == status.ToLower())
            .ToListAsync();
    }

    public async Task<RegisteredClient?> GetWithProductsAsync(Guid registrationId)
    {
        return await _dbSet
            .Include(c => c.RegisteredBy)
            .Include(c => c.IDSourceClient)
            .Include(c => c.ClientProducts)
                .ThenInclude(cp => cp.Product)
            .FirstOrDefaultAsync(c => c.RegistrationId == registrationId);
    }
}

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(IDVDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await _dbSet
            .Where(p => p.Category.ToLower() == category.ToLower() && p.IsActive)
            .OrderBy(p => p.ProductName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _dbSet
            .Where(p => p.IsActive)
            .OrderBy(p => p.Category)
            .ThenBy(p => p.ProductName)
            .ToListAsync();
    }

    public async Task<Product?> GetByProductCodeAsync(string productCode)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.ProductCode.ToLower() == productCode.ToLower());
    }
}

public class ClientProductRepository : Repository<ClientProduct>, IClientProductRepository
{
    public ClientProductRepository(IDVDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ClientProduct>> GetByClientAsync(Guid registrationId)
    {
        return await _dbSet
            .Include(cp => cp.Product)
            .Where(cp => cp.RegistrationId == registrationId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ClientProduct>> GetByProductAsync(Guid productId)
    {
        return await _dbSet
            .Include(cp => cp.RegisteredClient)
            .Where(cp => cp.ProductId == productId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ClientProduct>> GetByStatusAsync(string status)
    {
        return await _dbSet
            .Include(cp => cp.Product)
            .Include(cp => cp.RegisteredClient)
            .Where(cp => cp.Status.ToLower() == status.ToLower())
            .ToListAsync();
    }

    public async Task<ClientProduct?> GetClientProductAsync(Guid registrationId, Guid productId)
    {
        return await _dbSet
            .Include(cp => cp.Product)
            .FirstOrDefaultAsync(cp => cp.RegistrationId == registrationId && cp.ProductId == productId);
    }
}

public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(IDVDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId)
    {
        return await _dbSet
            .Include(a => a.User)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetByActionAsync(string action)
    {
        return await _dbSet
            .Include(a => a.User)
            .Where(a => a.Action.ToLower() == action.ToLower())
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(a => a.User)
            .Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
    }
}

public class VerificationAttemptRepository : Repository<VerificationAttempt>, IVerificationAttemptRepository
{
    public VerificationAttemptRepository(IDVDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<VerificationAttempt>> GetByUserAsync(Guid userId)
    {
        return await _dbSet
            .Include(v => v.User)
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.SearchTimestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<VerificationAttempt>> GetByIDNumberAsync(string idNumber)
    {
        return await _dbSet
            .Include(v => v.User)
            .Where(v => v.IDNumber == idNumber)
            .OrderByDescending(v => v.SearchTimestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<VerificationAttempt>> GetByResultStatusAsync(string resultStatus)
    {
        return await _dbSet
            .Include(v => v.User)
            .Where(v => v.ResultStatus.ToLower() == resultStatus.ToLower())
            .OrderByDescending(v => v.SearchTimestamp)
            .ToListAsync();
    }
}