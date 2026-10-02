

using Products.Domain.Entities;

namespace Products.Domain.RepositoryContracts;
public interface IProductsRepository
{
    Task<(int totalCount, IEnumerable<Product> products)> GetProductsAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid productId, CancellationToken cancellationToken );

    Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken);

    Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken);

    Task<Product?> UpdateProductAsync(Product updateProduct, CancellationToken cancellationToken);

    Task<Product?> GetProductAsync(Guid productId, CancellationToken cancellationToken);

    Task<(int totalCount, IEnumerable<Product> products)> SearchProducts(string searchTerm, int pageSize = 10, int pageNumber = 1, CancellationToken cancellationToken = default);
}
