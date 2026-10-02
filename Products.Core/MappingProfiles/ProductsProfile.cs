

using AutoMapper;
using Products.Core.Dtos;
using Products.Domain.Entities;

namespace Products.Core.MappingProfiles;
public class ProductsProfile : Profile
{
    public ProductsProfile() {
        CreateMap<Product, ProductsResponseDto>();
    }
}
