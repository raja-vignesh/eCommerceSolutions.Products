

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
        logger.LogInformation("GetProductsquery PageNumber:{PageNumber} PageSize:{PageSize}",request.PageNumber,request.PageSize);
        var (totalCount,products) = await productRepository.GetProductsAsync(request.PageSize, request.PageNumber, cancellationToken);
        logger.LogInformation("{Count} products fetched", totalCount);
        var result = mapper.Map<IEnumerable<ProductsResponseDto>>(products);
        return new PagedResult<ProductsResponseDto>(result,totalCount,request.PageNumber,request.PageSize);
        
    }
}
