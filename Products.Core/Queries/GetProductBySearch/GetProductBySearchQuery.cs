

using MediatR;
using Products.Core.Dtos;

namespace Products.Core.Queries.GetProductBySearch;
public class GetProductBySearchQuery(string searchTerm, int pageNumber, int pageSize) : IRequest<PagedResult<ProductsResponseDto>>
{
    public int PageNumber { get; set; } = pageNumber;

    public int PageSize { get; set; } = pageSize;
    public string SearchTerm { get; set; } = searchTerm;
}
