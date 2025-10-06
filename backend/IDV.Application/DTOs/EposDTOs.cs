using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IDV.Application.DTOs;

// EPOS Integration DTOs - Matching documentation format exactly
public class EposPayloadDto
{
    [JsonPropertyName("id_type")]
    public string IdType { get; set; } = string.Empty;
    
    [JsonPropertyName("id_number")]
    public string IdNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;
    
    [JsonPropertyName("date_of_birth")]
    public string DateOfBirth { get; set; } = string.Empty;
    
    [JsonPropertyName("gender")]
    public string Gender { get; set; } = string.Empty;
    
    [JsonPropertyName("mobile_number")]
    public string MobileNumber { get; set; } = string.Empty;
    
    [JsonPropertyName("address")]
    public EposAddressDto Address { get; set; } = new();
    
    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;
    
    [JsonPropertyName("captured_by")]
    public string CapturedBy { get; set; } = "EPOS-DB";
    
    [JsonPropertyName("capture_timestamp")]
    public string CaptureTimestamp { get; set; } = string.Empty;
    
    [JsonPropertyName("products")]
    public List<EposProductDto>? Products { get; set; }
}

public class EposAddressDto
{
    [JsonPropertyName("province")]
    public string Province { get; set; } = string.Empty;
    
    [JsonPropertyName("district")]
    public string District { get; set; } = string.Empty;
    
    [JsonPropertyName("postal_code")]
    public string PostalCode { get; set; } = string.Empty;
}

public class EposProductDto
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public decimal PremiumAmount { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ClientRegistrationWithEposDto : RegisterClientResponseDto
{
    public EposPayloadDto EposPayload { get; set; } = new();
}