namespace Products.Domain.Exceptions.NotFound;
public class ProductsNotFoundException : NotFoundException
{
    public ProductsNotFoundException() : base("No Products Found")
    {
    }
}
