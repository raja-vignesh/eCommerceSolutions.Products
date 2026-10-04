

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Products.Core.Dtos;
using Products.Domain.Entities;
using Products.Domain.RepositoryContracts;

namespace Products.Core.Commands.UpdateProduct;
public class UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger, IProductsRepository productsRepository, IMapper mapper) : IRequestHandler<UpdateProductCommand, ProductsResponseDto>
{
    public async Task<ProductsResponseDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("update product request: {@request}" , request);
        var product = mapper.Map<Product>(request);
        var updated = await productsRepository.UpdateProductAsync(product, cancellationToken);
        return mapper.Map<ProductsResponseDto>(updated);
    }
}
