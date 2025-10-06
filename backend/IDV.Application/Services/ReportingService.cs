using AutoMapper;
using IDV.Application.DTOs;
using IDV.Application.Interfaces;
using IDV.Core.Interfaces;

namespace IDV.Application.Services;

public class ReportingService : IReportingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReportingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClientReportDto>> GenerateClientReportAsync(ExportRequestDto? filters = null)
    {
        // Get all clients with their relationships
        var clients = await _unitOfWork.RegisteredClients.GetAllAsync();

        // Apply simple filters (in a real app, you'd want to do this at the database level)
        if (filters != null)
        {
            if (filters.StartDate.HasValue)
                clients = clients.Where(c => c.RegistrationDate >= filters.StartDate.Value);

            if (filters.EndDate.HasValue)
                clients = clients.Where(c => c.RegistrationDate <= filters.EndDate.Value);

            if (!string.IsNullOrEmpty(filters.Province))
                clients = clients.Where(c => c.Province.Contains(filters.Province));

            if (!string.IsNullOrEmpty(filters.Status))
                clients = clients.Where(c => c.Status == filters.Status);
        }

        var clientProducts = await _unitOfWork.ClientProducts.GetAllAsync();
        
        return clients.Select(c => new ClientReportDto
        {
            RegistrationId = c.RegistrationId,
            IDNumber = c.IDNumber,
            FullName = c.FullName,
            Email = c.Email,
            MobileNumber = c.MobileNumber,
            Province = c.Province,
            Status = c.Status,
            RegistrationDate = c.RegistrationDate,
            ProductCount = clientProducts.Count(cp => cp.RegistrationId == c.RegistrationId),
            TotalPremium = clientProducts.Where(cp => cp.RegistrationId == c.RegistrationId).Sum(cp => cp.PremiumAmount),
            RegisteredBy = "System" // Simplified for now
        });
    }

    public async Task<ExportResponseDto> ExportToExcelAsync(ExportRequestDto request)
    {
        // This would typically generate an actual Excel file
        // For now, we'll return a mock response
        var clients = await GenerateClientReportAsync(request);
        
        return new ExportResponseDto
        {
            Success = true,
            FileName = "clients_report.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };
    }

    public async Task<ExportResponseDto> ExportToPdfAsync(ExportRequestDto request)
    {
        // This would typically generate an actual PDF file
        // For now, we'll return a mock response
        var clients = await GenerateClientReportAsync(request);
        
        return new ExportResponseDto
        {
            Success = true,
            FileName = "clients_report.pdf",
            ContentType = "application/pdf"
        };
    }

    public async Task<DashboardStatisticsDto> GetDashboardStatisticsAsync()
    {
        var clients = await _unitOfWork.RegisteredClients.GetAllAsync();
        var products = await _unitOfWork.Products.GetAllAsync();
        var clientProducts = await _unitOfWork.ClientProducts.GetAllAsync();

        // Calculate province statistics
        var provinceStats = clients
            .GroupBy(c => c.Province)
            .Select(g => new ProvinceStatDto
            {
                Province = g.Key,
                ClientCount = g.Count(),
                TotalPremium = clientProducts.Where(cp => g.Any(c => c.RegistrationId == cp.RegistrationId)).Sum(cp => cp.PremiumAmount)
            })
            .OrderByDescending(p => p.ClientCount)
            .ToList();

        // Calculate category statistics
        var categoryStats = products
            .GroupBy(p => p.Category)
            .Select(g => new ProductCategoryStatDto
            {
                Category = g.Key,
                ProductCount = g.Count(),
                EnrollmentCount = clientProducts.Count(cp => g.Any(p => p.ProductId == cp.ProductId)),
                TotalPremium = clientProducts.Where(cp => g.Any(p => p.ProductId == cp.ProductId)).Sum(cp => cp.PremiumAmount)
            })
            .OrderByDescending(c => c.EnrollmentCount)
            .ToList();

        // Calculate registration trends (last 30 days)
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        var registrationTrends = clients
            .Where(c => c.RegistrationDate >= thirtyDaysAgo)
            .GroupBy(c => c.RegistrationDate.Date)
            .Select(g => new RegistrationTrendDto
            {
                Date = g.Key,
                RegistrationCount = g.Count(),
                Revenue = clientProducts.Where(cp => g.Any(c => c.RegistrationId == cp.RegistrationId)).Sum(cp => cp.PremiumAmount)
            })
            .OrderBy(r => r.Date)
            .ToList();

        var thisMonth = DateTime.UtcNow.AddDays(-30);
        var today = DateTime.UtcNow.Date;
        var todayRegistrations = clients.Count(c => c.RegistrationDate.Date == today);
        
        // Get verification statistics
        var verificationAttempts = await _unitOfWork.VerificationAttempts.GetAllAsync();
        var todayVerifications = verificationAttempts.Count(v => v.SearchTimestamp.Date == today);
        var successfulVerifications = verificationAttempts.Count(v => v.ResultStatus == "Found");
        var totalVerifications = verificationAttempts.Count();
        var averageResponseTime = totalVerifications > 0 ? verificationAttempts.Average(v => v.ResponseTime) : 0;
        var successRate = totalVerifications > 0 ? (decimal)successfulVerifications / totalVerifications * 100 : 0;
        
        // Get top verification sources
        var topSources = verificationAttempts
            .GroupBy(v => v.SourceSystem)
            .Select(g => new VerificationSourceStatDto
            {
                Source = g.Key,
                Count = g.Count(),
                Percentage = totalVerifications > 0 ? (decimal)g.Count() / totalVerifications * 100 : 0
            })
            .OrderByDescending(s => s.Count)
            .Take(5)
            .ToList();
        
        // Get product statistics
        var allProducts = await _unitOfWork.Products.GetAllAsync();
        var activeProducts = allProducts.Count(p => p.IsActive);

        return new DashboardStatisticsDto
        {
            TotalClients = clients.Count(),
            TodayRegistrations = todayRegistrations,
            TotalVerifications = totalVerifications,
            TodayVerifications = todayVerifications,
            TotalProducts = allProducts.Count(),
            ActiveProducts = activeProducts,
            AverageResponseTime = (decimal)(averageResponseTime / 1000), // Convert to seconds
            SuccessRate = successRate,
            TopVerificationSources = topSources,
            ProvinceStats = provinceStats,
            CategoryStats = categoryStats,
            RegistrationTrends = registrationTrends
        };
    }
}