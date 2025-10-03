using AutoMapper;
using IDV.Application.DTOs;
using IDV.Core.Entities;

namespace IDV.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();
        
        // IDSourceClient mappings
        CreateMap<IDSourceClient, ClientSearchResultDto>();
        
        // RegisteredClient mappings
        CreateMap<RegisteredClient, ClientDetailsDto>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ClientProducts));
        CreateMap<RegisterClientRequestDto, RegisteredClient>();
        
        // Product mappings
        CreateMap<Product, ProductDto>();
        CreateMap<ClientProduct, ClientProductDto>();
        
        // Report mappings
        CreateMap<RegisteredClient, ClientReportDto>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.ClientProducts.Count))
            .ForMember(dest => dest.TotalPremium, opt => opt.MapFrom(src => src.ClientProducts.Sum(cp => cp.PremiumAmount)))
            .ForMember(dest => dest.RegisteredBy, opt => opt.MapFrom(src => src.RegisteredBy.FullName));
    }
}