

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Products.Core.Dtos;
using Products.Domain.Entities;
using Products.Domain.RepositoryContracts;

namespace Products.Core.Commands.CreateProduct;
public class CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, IMapper mapper, IProductsRepository productsRepository) : IRequestHandler<CreateProductCommand, ProductsResponseDto>
{
    public async Task<ProductsResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Create Product request {@request}", request);
        var product = mapper.Map<Product>(request);
        product.ProductId = Guid.NewGuid();
        var created =  await productsRepository.AddProductAsync(product, cancellationToken);
        return mapper.Map<ProductsResponseDto>(product);
    }
}
