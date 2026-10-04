

using MediatR;

namespace Products.Core.Commands.DeleteProduct;
public class DeleteProductCommand(Guid productId) : IRequest
{
    public Guid ProductId { get; } = productId; 
}
