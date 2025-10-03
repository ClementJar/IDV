using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using IDV.Application.DTOs;
using IDV.Application.Interfaces;
using IDV.Core.Entities;
using IDV.Core.Interfaces;

namespace IDV.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Account is disabled");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        var refreshToken = Guid.NewGuid().ToString();

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
    {
        // In a real application, you would validate the refresh token
        throw new NotImplementedException("Refresh token functionality not implemented for demo");
    }

    public async Task<UserDto?> GetCurrentUserAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public Task LogoutAsync(string token)
    {
        // In a real application, you would blacklist the token
        return Task.CompletedTask;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var key = Encoding.ASCII.GetBytes(secretKey!);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"]!)),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class IDVerificationService : IIDVerificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly Random _random = new();

    public IDVerificationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDVerificationResponseDto> VerifyIDNumberAsync(string idNumber, Guid userId)
    {
        var startTime = DateTime.UtcNow;
        
        // Simulate network delay
        await Task.Delay(_random.Next(200, 500));
        
        var results = await _unitOfWork.IDSourceClients.SearchByIDNumberAsync(idNumber);
        var clientResults = results.ToList();
        
        var responseTime = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;
        var status = clientResults.Count switch
        {
            0 => "NotFound",
            1 => "Found",
            > 1 => "Multiple",
            _ => "Error"
        };

        await LogVerificationAttemptAsync(userId, idNumber, status, clientResults.Count, responseTime, "Mock_API");

        return new IDVerificationResponseDto
        {
            Success = clientResults.Any(),
            Status = status,
            ResultCount = clientResults.Count,
            ResponseTime = responseTime,
            Source = "Mock External API",
            Results = _mapper.Map<List<ClientSearchResultDto>>(clientResults)
        };
    }

    public Task<IDVerificationResponseDto> SearchMultipleSourcesAsync(string idNumber, Guid userId)
    {
        return VerifyIDNumberAsync(idNumber, userId);
    }

    public async Task<MultiSourceVerificationResponseDto> SearchMultipleSourcesWithProgressAsync(string idNumber, Guid userId)
    {
        var startTime = DateTime.UtcNow;
        
        var dataSources = new List<(string Source, string DisplayName, int Priority)>
        {
            ("INRIS", "ID Registration Information System", 1),
            ("ZRA", "Zambia Revenue Authority", 2),
            ("MNO_AIRTEL", "Airtel Network Database", 3),
            ("MNO_MTN", "MTN Network Database", 4),
            ("MNO_ZAMTEL", "Zamtel Network Database", 5),
            ("BANK_ZANACO", "Zanaco Banking Records", 6),
            ("BANK_FNB", "FNB Banking Records", 7),
            ("BANK_STANCHART", "Standard Chartered Records", 8),
            ("GOVT_PAYROLL", "Government Payroll System", 9),
            ("NAPSA", "National Pension Scheme Authority", 10),
            ("RTSA", "Road Transport & Safety Agency", 11)
        };

        var response = new MultiSourceVerificationResponseDto
        {
            IDNumber = idNumber,
            SourceResults = new List<SourceSearchResultDto>(),
            Success = false,
            OverallStatus = "Searching"
        };

        ClientSearchResultDto? foundResult = null;
        var totalResponseTime = 0;

        // Search through sources in priority order
        foreach (var (source, displayName, priority) in dataSources.OrderBy(x => x.Priority))
        {
            var sourceStart = DateTime.UtcNow;
            
            var sourceResult = new SourceSearchResultDto
            {
                SourceName = source,
                DisplayName = displayName,
                Priority = priority,
                Status = "Checking"
            };
            
            response.SourceResults.Add(sourceResult);

            try
            {
                // Search in this specific source - optimized database query
                var clientResult = await _unitOfWork.IDSourceClients.GetByIDAndSourceAsync(idNumber, source);
                
                var responseTime = (int)(DateTime.UtcNow - sourceStart).TotalMilliseconds;
                totalResponseTime += responseTime;
                
                sourceResult.ResponseTime = responseTime;
                
                if (clientResult != null)
                {
                    sourceResult.Status = "Found";
                    sourceResult.IsFound = true;
                    sourceResult.Result = _mapper.Map<ClientSearchResultDto>(clientResult);
                    foundResult = sourceResult.Result;
                    
                    // Log successful verification
                    await LogVerificationAttemptAsync(userId, idNumber, "Found", 1, responseTime, source);
                    
                    // Found in this source, we can stop searching
                    break;
                }
                else
                {
                    sourceResult.Status = "NotFound";
                    sourceResult.IsFound = false;
                }
            }
            catch (Exception ex)
            {
                sourceResult.Status = "Error";
                sourceResult.ErrorMessage = ex.Message;
                sourceResult.ResponseTime = (int)(DateTime.UtcNow - sourceStart).TotalMilliseconds;
                totalResponseTime += sourceResult.ResponseTime;
            }
        }

        // Mark remaining sources as not checked if we found a result
        if (foundResult != null)
        {
            foreach (var remaining in response.SourceResults.Where(s => s.Status == "Checking"))
            {
                remaining.Status = "Skipped";
            }
        }

        response.Success = foundResult != null;
        response.FinalResult = foundResult;
        response.TotalResponseTime = totalResponseTime;
        response.OverallStatus = foundResult != null ? "Found" : "NotFound";

        return response;
    }

    public async Task LogVerificationAttemptAsync(Guid userId, string idNumber, string resultStatus, int resultCount, int responseTime, string sourceSystem)
    {
        var attempt = new VerificationAttempt
        {
            UserId = userId,
            IDNumber = idNumber,
            ResultStatus = resultStatus,
            ResultCount = resultCount,
            ResponseTime = responseTime,
            SourceSystem = sourceSystem,
            SearchTimestamp = DateTime.UtcNow
        };

        await _unitOfWork.VerificationAttempts.AddAsync(attempt);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<AvailableTestIdDto>> GetAvailableTestIdsAsync()
    {
        // Get all ID source clients
        var allSourceClients = await _unitOfWork.IDSourceClients.GetAllAsync();
        
        // Get all registered clients to exclude them
        var registeredClients = await _unitOfWork.RegisteredClients.GetAllAsync();
        var registeredIdNumbers = registeredClients.Select(c => c.IDNumber).ToHashSet();
        
        // Filter out already registered IDs and get sample test IDs
        var availableTestIds = allSourceClients
            .Where(c => !registeredIdNumbers.Contains(c.IDNumber))
            .GroupBy(c => new { c.IDType, c.Source })
            .SelectMany(g => g.Take(1)) // Take 1 ID per ID type and source combination
            .Take(8) // Limit to 8 total test IDs for variety
            .Select(c => new AvailableTestIdDto
            {
                IdNumber = c.IDNumber,
                FullName = c.FullName,
                Source = c.Source,
                DisplaySource = GetDisplaySourceName(c.Source)
            })
            .OrderBy(c => c.Source)
            .ThenBy(c => c.IdNumber)
            .ToList();
            
        return availableTestIds;
    }

    private static string GetDisplaySourceName(string source)
    {
        return source switch
        {
            "INRIS" => "INRIS",
            "ZRA" => "ZRA", 
            "MTN" => "MTN",
            "Airtel" => "Airtel",
            "Zamtel" => "Zamtel",
            "Zanaco" => "Zanaco",
            "FNB" => "FNB",
            "Standard_Bank" => "Standard Bank",
            "NAPSA" => "NAPSA",
            "ECZ" => "ECZ",
            "PASSPORT_OFFICE" => "Passport Office",
            "RTSA" => "Road Transport & Safety Agency",
            _ => source
        };
    }
}

public class ClientRegistrationService : IClientRegistrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClientRegistrationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RegisterClientResponseDto> RegisterNewClientAsync(RegisterClientRequestDto request, Guid registeredByUserId)
    {
        var existingClient = await _unitOfWork.RegisteredClients.GetSingleAsync(c => c.IDNumber == request.IDNumber);
        if (existingClient != null)
        {
            return new RegisterClientResponseDto
            {
                Success = false,
                Message = "Client with this ID number is already registered"
            };
        }

        var client = _mapper.Map<RegisteredClient>(request);
        client.RegisteredByUserId = registeredByUserId;
        client.RegistrationDate = DateTime.UtcNow;
        client.Status = "Active";

        await _unitOfWork.RegisteredClients.AddAsync(client);
        await _unitOfWork.SaveChangesAsync();

        // Attach products if provided
        if (request.ProductIds.Any())
        {
            foreach (var productId in request.ProductIds)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product != null)
                {
                    var clientProduct = new ClientProduct
                    {
                        RegistrationId = client.RegistrationId,
                        ProductId = productId,
                        PremiumAmount = product.PremiumAmount,
                        PolicyNumber = GeneratePolicyNumber(),
                        StartDate = DateTime.UtcNow,
                        Status = "Active"
                    };
                    await _unitOfWork.ClientProducts.AddAsync(clientProduct);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        var clientDetails = await GetClientDetailsAsync(client.RegistrationId);
        
        return new RegisterClientResponseDto
        {
            Success = true,
            Message = "Client registered successfully",
            RegistrationId = client.RegistrationId,
            Client = clientDetails
        };
    }

    public async Task<ClientDetailsDto?> GetClientDetailsAsync(Guid registrationId)
    {
        var client = await _unitOfWork.RegisteredClients.GetWithProductsAsync(registrationId);
        return client != null ? _mapper.Map<ClientDetailsDto>(client) : null;
    }

    public async Task<ClientDetailsDto> UpdateClientInfoAsync(Guid registrationId, UpdateClientDto request)
    {
        var client = await _unitOfWork.RegisteredClients.GetByIdAsync(registrationId);
        if (client == null)
            throw new ArgumentException("Client not found");

        client.FullName = request.FullName;
        client.MobileNumber = request.MobileNumber;
        client.Email = request.Email;
        client.Province = request.Province;
        client.District = request.District;
        client.PostalCode = request.PostalCode;
        client.Status = request.Status;
        client.Notes = request.Notes;

        await _unitOfWork.RegisteredClients.UpdateAsync(client);
        await _unitOfWork.SaveChangesAsync();

        return (await GetClientDetailsAsync(registrationId))!;
    }

    public async Task<IEnumerable<ClientDetailsDto>> GetAllRegisteredClientsAsync()
    {
        var clients = await _unitOfWork.RegisteredClients.GetAllAsync();
        return _mapper.Map<IEnumerable<ClientDetailsDto>>(clients);
    }

    public async Task<IEnumerable<ClientDetailsDto>> SearchRegisteredClientsAsync(string searchTerm)
    {
        var clients = await _unitOfWork.RegisteredClients.SearchClientsAsync(searchTerm);
        return _mapper.Map<IEnumerable<ClientDetailsDto>>(clients);
    }

    public async Task<bool> DeleteClientAsync(Guid registrationId)
    {
        var client = await _unitOfWork.RegisteredClients.GetByIdAsync(registrationId);
        if (client == null) return false;

        await _unitOfWork.RegisteredClients.DeleteAsync(client);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private string GeneratePolicyNumber()
    {
        return $"POL{DateTime.UtcNow:yyyyMMdd}{new Random().Next(1000, 9999)}";
    }
}