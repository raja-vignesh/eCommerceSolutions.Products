
using MediatR;
using Products.Core.Dtos;

namespace Products.Core.Queries.GetProductById;
public class GetProductByIdQuery(Guid productId) : IRequest<ProductsResponseDto>
{
    public Guid ProudctId { get; set; } = productId;
}
