
namespace Products.Domain.Entities;
public class Product
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = default!;

    public string Category { get; set; } = default!;

    public double UnitPrice { get; set; }

    public int QuantityInStock  { get; set; }
}
