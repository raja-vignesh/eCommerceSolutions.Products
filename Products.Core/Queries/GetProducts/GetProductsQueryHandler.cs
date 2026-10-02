

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Products.Core.Dtos;
using Products.Domain.Exceptions.NotFound;
using Products.Domain.RepositoryContracts;

namespace Products.Core.Queries.GetProducts;
public class GetProductsQueryHandler(IProductsRepository productRepository,ILogger<GetProductsQueryHandler> logger,IMapper mapper) : IRequestHandler<GetProductsQuery, PagedResult< ProductsResponseDto>>
{
   
    public async Task<PagedResult<ProductsResponseDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProducts query");
        var products = await productRepository.GetProductsAsync(request.PageSize, request.PageNumber, cancellationToken);
        logger.LogInformation("{Count} products fetched", products.totalCount);
        var result = mapper.Map<IEnumerable<ProductsResponseDto>>(products.products);
        var pagedResult = new PagedResult<ProductsResponseDto>(result,products.totalCount,request.PageNumber,request.PageSize);
        return pagedResult;
    }
}
