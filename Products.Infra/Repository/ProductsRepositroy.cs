

using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Infra.Repository;
public class ProductsRepositroy(ApplicationDbContext applicationDbContext)
{
    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await applicationDbContext.Products.AsNoTracking().ToListAsync(cancellationToken);
        return products;
    }

    public async Task<bool> ExistsAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await applicationDbContext.Products.AnyAsync(x => x.ProductId == productId, cancellationToken);
    }

    public async Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var product = await applicationDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        if (product != null)
        {
            applicationDbContext.Products.Remove(product);
            await applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        await applicationDbContext.Products.AddAsync(product,cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product?> UpdateProductAsync(Product updateProduct, CancellationToken cancellationToken = default)
    {
        var product = await applicationDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == updateProduct.ProductId, cancellationToken);
        if (product != null)
        {
            product.ProductName = updateProduct.ProductName;
            product.UnitPrice = updateProduct.UnitPrice;
            product.QuantityInStock = updateProduct.QuantityInStock;
            product.Category = updateProduct.Category;
            await applicationDbContext.SaveChangesAsync(cancellationToken);
            return product;
        }
        return null;
    }

} 
