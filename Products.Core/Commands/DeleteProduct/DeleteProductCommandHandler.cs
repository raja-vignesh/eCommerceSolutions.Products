
using MediatR;
using Microsoft.Extensions.Logging;
using Products.Domain.Exceptions.NotFound;
using Products.Domain.RepositoryContracts;

namespace Products.Core.Commands.DeleteProduct;
public class DeleteProductCommandHandler(ILogger<DeleteProductCommandHandler> logger, IProductsRepository productsRepository) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Delete product with id: {Id}", request.ProductId);
        var exists = await productsRepository.ExistsAsync(request.ProductId, cancellationToken);
        if (!exists) throw new ProductNotFoundException(request.ProductId);     
        await productsRepository.DeleteProductAsync(request.ProductId, cancellationToken);
    }
}
