

using AutoMapper;
using Products.Core.Commands.CreateProduct;
using Products.Core.Dtos;
using Products.Domain.Entities;

namespace Products.Core.MappingProfiles;
public class ProductsProfile : Profile
{
    public ProductsProfile() {
        CreateMap<Product, ProductsResponseDto>();
        CreateMap<CreateProductCommand, Product>();
            //.ForMember(p => p.Category, opt => opt.MapFrom(src => src.Category.ToString()));
    }
}
