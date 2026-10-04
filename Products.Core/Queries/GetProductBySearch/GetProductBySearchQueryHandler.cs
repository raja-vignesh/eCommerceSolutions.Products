

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Products.Core.Dtos;
using Products.Domain.RepositoryContracts;

namespace Products.Core.Queries.GetProductBySearch;
public class GetProductBySearchQueryHandler(ILogger<GetProductBySearchQueryHandler> logger, IMapper mapper, IProductsRepository productsRepository) : IRequestHandler<GetProductBySearchQuery, PagedResult<ProductsResponseDto>>
{
    public async Task<PagedResult<ProductsResponseDto>> Handle(GetProductBySearchQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Search products with search term: {Term}", request.SearchTerm);
        var (totalCount,products) = await productsRepository.SearchProducts(request.SearchTerm, request.PageSize, request.PageNumber, cancellationToken);
        logger.LogInformation("{Count} products fetched", totalCount);
        var result = mapper.Map<IEnumerable<ProductsResponseDto>>(products);
        return new PagedResult<ProductsResponseDto>(result, totalCount, request.PageNumber, request.PageSize);
    }
}

