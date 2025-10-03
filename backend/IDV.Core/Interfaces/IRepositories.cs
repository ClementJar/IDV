using IDV.Core.Entities;

namespace IDV.Core.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ValidatePasswordAsync(string username, string password);
}

public interface IIDSourceRepository : IRepository<IDSourceClient>
{
    Task<IEnumerable<IDSourceClient>> SearchByIDNumberAsync(string idNumber);
    Task<IDSourceClient?> GetByIDNumberAsync(string idNumber);
    Task<IEnumerable<IDSourceClient>> GetByProvinceAsync(string province);
    Task<IDSourceClient?> GetByIDAndSourceAsync(string idNumber, string source);
}

public interface IRegisteredClientRepository : IRepository<RegisteredClient>
{
    Task<IEnumerable<RegisteredClient>> SearchClientsAsync(string searchTerm);
    Task<IEnumerable<RegisteredClient>> GetByRegisteredByAsync(Guid userId);
    Task<IEnumerable<RegisteredClient>> GetByStatusAsync(string status);
    Task<RegisteredClient?> GetWithProductsAsync(Guid registrationId);
}

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    Task<IEnumerable<Product>> GetActiveProductsAsync();
    Task<Product?> GetByProductCodeAsync(string productCode);
}

public interface IClientProductRepository : IRepository<ClientProduct>
{
    Task<IEnumerable<ClientProduct>> GetByClientAsync(Guid registrationId);
    Task<IEnumerable<ClientProduct>> GetByProductAsync(Guid productId);
    Task<IEnumerable<ClientProduct>> GetByStatusAsync(string status);
    Task<ClientProduct?> GetClientProductAsync(Guid registrationId, Guid productId);
}

public interface IAuditLogRepository : IRepository<AuditLog>
{
    Task<IEnumerable<AuditLog>> GetByUserAsync(Guid userId);
    Task<IEnumerable<AuditLog>> GetByActionAsync(string action);
    Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}

public interface IVerificationAttemptRepository : IRepository<VerificationAttempt>
{
    Task<IEnumerable<VerificationAttempt>> GetByUserAsync(Guid userId);
    Task<IEnumerable<VerificationAttempt>> GetByIDNumberAsync(string idNumber);
    Task<IEnumerable<VerificationAttempt>> GetByResultStatusAsync(string resultStatus);
}