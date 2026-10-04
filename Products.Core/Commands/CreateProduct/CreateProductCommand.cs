
using MediatR;
using Products.Core.Dtos;
using Products.Core.enums;

namespace Products.Core.Commands.CreateProduct;
public class CreateProductCommand : IRequest<ProductsResponseDto>
{
    public string ProductName { get; set; } = default!;

    public CategoryOptions Category { get; set; } = default!;

    public double UnitPrice { get; set; }

    public int QuantityInStock { get; set; }
}
