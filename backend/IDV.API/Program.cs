using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using IDV.Application.Interfaces;
using IDV.Application.Services;
using IDV.Application.Mappings;
using IDV.Core.Interfaces;
using IDV.Infrastructure.Data;
using IDV.Infrastructure.Repositories;
using IDV.Infrastructure.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Npgsql to handle DateTime as UTC
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Get connection string and validate
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string 'DefaultConnection' is not configured. Please set the ConnectionStrings__DefaultConnection environment variable.");
}

builder.Services.AddDbContext<IDVDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add health checks
builder.Services.AddHealthChecks();

// Add repositories and unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add application services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IIDVerificationService, IDVerificationService>();
builder.Services.AddScoped<IClientRegistrationService, ClientRegistrationService>();
builder.Services.AddScoped<IReportingService, ReportingService>();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // Get allowed origins from configuration (supports both array and individual env vars)
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new string[0];
        
        // Also check for individual environment variables (AllowedOrigins__0, AllowedOrigins__1, etc.)
        var envOrigins = new List<string>();
        for (int i = 0; i < 10; i++) // Check up to 10 origins
        {
            var origin = builder.Configuration[$"AllowedOrigins__{i}"];
            if (!string.IsNullOrEmpty(origin))
                envOrigins.Add(origin);
        }
        
        // Always include the production frontend URL
        var productionOrigins = new[] { "https://idv-front.onrender.com" };
        
        // Combine all sources
        var allOrigins = allowedOrigins.Concat(envOrigins).Concat(productionOrigins).Distinct().ToArray();
        
        if (allOrigins.Length > 0)
        {
            policy.WithOrigins(allOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        else
        {
            // Fallback for development
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IDV API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Add detailed logging
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Starting IDV API application...");
logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

// Test database connection
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<IDVDbContext>();
        logger.LogInformation("Testing database connection...");
        
        // Test if we can connect to the database
        await context.Database.CanConnectAsync();
        logger.LogInformation("Database connection successful!");
        
        // Seed database
        logger.LogInformation("Starting database seeding...");
        await DatabaseSeeder.SeedAsync(context);
        logger.LogInformation("Database seeding completed successfully!");
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "Database error occurred: {Message}", ex.Message);
    logger.LogError("Connection string: {ConnectionString}", 
        builder.Configuration.GetConnectionString("DefaultConnection")?.Substring(0, 50) + "...");
    
    // Continue running the app even if database fails in production
    if (app.Environment.IsDevelopment())
    {
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// Map health check endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
