

using Products.Core.enums;

namespace Products.Core.Dtos;
public class ProductsResponseDto
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = default!;

    public CategoryOptions Category { get; set; } = default!;

    public decimal UnitPrice { get; set; }

    public int QuantityInStock { get; set; }
}
