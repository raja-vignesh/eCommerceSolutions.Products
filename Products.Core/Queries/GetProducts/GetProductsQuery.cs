

using MediatR;
using Products.Core.Dtos;

namespace Products.Core.Queries.GetProducts;
public class GetProductsQuery(int pageNumber, int pageSize) : IRequest<PagedResult<ProductsResponseDto>>
{
    public int PageNumber = pageNumber;
    public int PageSize = pageSize;
}
