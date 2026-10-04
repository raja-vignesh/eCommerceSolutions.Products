

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Products.Core.Dtos;
using Products.Domain.Exceptions.NotFound;
using Products.Domain.RepositoryContracts;

namespace Products.Core.Queries.GetProductById;
public class GetProductByIdQueryHandler(IProductsRepository productsRepository, ILogger<GetProductByIdQueryHandler> logger, IMapper mapper) : IRequestHandler<GetProductByIdQuery, ProductsResponseDto>
{
    public async Task<ProductsResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetch Product with id: {Id}", request.ProudctId);
        var product = await productsRepository.GetProductAsync(request.ProudctId, cancellationToken);
        if (product == null) { throw new ProductNotFoundException(request.ProudctId); }
        return mapper.Map<ProductsResponseDto>(product);        
    }
}
