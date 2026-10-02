

using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Infra.Repository;
public class ProductsRepository(ApplicationDbContext applicationDbContext)
{
    public async Task<(int totalCount,IEnumerable<Product> products)> GetProductsAsync(int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var query =  applicationDbContext.Products.AsNoTracking();
        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query.OrderBy(p => p.ProductId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (totalCount,products);
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

    public async Task<Product?> GetProductAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await applicationDbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == productId,cancellationToken);     
    }

    public async Task<(int totalCount,IEnumerable<Product> products)> SearchProducts( string searchTerm, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = applicationDbContext.Products.AsNoTracking();
        if ( !string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();
            query = query.Where(p => p.ProductName.Contains(searchTerm));
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var products = await query.OrderBy(p => p.ProductId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return (totalCount,products);
    }

} 
