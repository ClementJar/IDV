using IDV.Application.DTOs;
using IDV.Core.Entities;

namespace IDV.Application.Interfaces;

public interface IAuthenticationService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<TokenDto> RefreshTokenAsync(string refreshToken);
    Task<UserDto?> GetCurrentUserAsync(Guid userId);
    Task LogoutAsync(string token);
}

public interface IIDVerificationService
{
    Task<IDVerificationResponseDto> VerifyIDNumberAsync(string idNumber, Guid userId);
    Task<IDVerificationResponseDto> SearchMultipleSourcesAsync(string idNumber, Guid userId);
    Task<MultiSourceVerificationResponseDto> SearchMultipleSourcesWithProgressAsync(string idNumber, Guid userId);
    Task LogVerificationAttemptAsync(Guid userId, string idNumber, string resultStatus, int resultCount, int responseTime, string sourceSystem);
    Task<List<AvailableTestIdDto>> GetAvailableTestIdsAsync();
}

public interface IClientRegistrationService
{
    Task<ClientRegistrationWithEposDto> RegisterNewClientAsync(RegisterClientRequestDto request, Guid registeredByUserId);
    Task<ClientDetailsDto?> GetClientDetailsAsync(Guid registrationId);
    Task<ClientDetailsDto> UpdateClientInfoAsync(Guid registrationId, UpdateClientDto request);
    Task<IEnumerable<ClientDetailsDto>> GetAllRegisteredClientsAsync();
    Task<IEnumerable<ClientDetailsDto>> SearchRegisteredClientsAsync(string searchTerm);
    Task<bool> DeleteClientAsync(Guid registrationId);
}

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
    Task<ProductDto?> GetProductByIdAsync(Guid productId);
    Task<ClientProductDto> AttachProductToClientAsync(AttachProductRequestDto request);
    Task<bool> RemoveProductFromClientAsync(Guid clientProductId);
    Task<IEnumerable<ClientProductDto>> GetClientProductsAsync(Guid registrationId);
    Task<ClientProductDto> UpdateClientProductAsync(Guid clientProductId, UpdateClientProductDto request);
    Task<IEnumerable<string>> GetProductCategoriesAsync();
}

public interface IReportingService
{
    Task<IEnumerable<ClientReportDto>> GenerateClientReportAsync(ExportRequestDto? filters = null);
    Task<ExportResponseDto> ExportToExcelAsync(ExportRequestDto request);
    Task<ExportResponseDto> ExportToPdfAsync(ExportRequestDto request);
    Task<DashboardStatisticsDto> GetDashboardStatisticsAsync();
}

public interface IAuditService
{
    Task LogActionAsync(Guid userId, string action, string entityType, Guid? entityId = null, string? details = null, string? ipAddress = null);
    Task<IEnumerable<AuditLog>> GetAuditTrailAsync(DateTime? startDate = null, DateTime? endDate = null, Guid? userId = null);
}