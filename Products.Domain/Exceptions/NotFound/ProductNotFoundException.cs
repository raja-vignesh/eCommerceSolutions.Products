namespace Products.Domain.Exceptions.NotFound;
public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid productId) : base($"{productId} not found")
    {
    }
}
