

using MediatR;
using Products.Core.Dtos;
using Products.Core.enums;

namespace Products.Core.Commands.UpdateProduct;
public class UpdateProductCommand : IRequest<ProductsResponseDto>
{
    public Guid ProductId { get; set; } = Guid.Empty;
    public string ProductName { get; set; } = default!;

    public CategoryOptions Category { get; set; } = default!;

    public decimal UnitPrice { get; set; }

    public int QuantityInStock { get; set; }
}
