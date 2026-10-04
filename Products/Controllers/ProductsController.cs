using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Core.Commands.CreateProduct;
using Products.Core.Commands.DeleteProduct;
using Products.Core.Dtos;
using Products.Core.Queries.GetProductById;
using Products.Core.Queries.GetProductBySearch;
using Products.Core.Queries.GetProducts;

namespace Products.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator, ILogger<ProductsController> logger) : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProductsResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedResult<ProductsResponseDto>>> Get(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetProductsQuery(pageNumber, pageSize);
        var results = await mediator.Send(query, cancellationToken);
        return Ok(results);
    }

    [HttpGet("{productId:guid}")]
    [ProducesResponseType(typeof(ProductsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status404NotFound)]

    public async Task<ActionResult<ProductsResponseDto>> Get(Guid productId, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(productId);
        return Ok(await mediator.Send(query, cancellationToken));
    }


    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<ProductsResponseDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(
    typeof(ProblemDetails),
    StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<ProductsResponseDto>>> Search([FromQuery]string searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetProductBySearchQuery(searchTerm, pageNumber, pageSize);
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductsResponseDto),statusCode:StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(
    typeof(ProblemDetails),
    StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductsResponseDto>> Create(CreateProductCommand createProductCommand,CancellationToken cancellationToken)
    {
        var created = await mediator.Send(createProductCommand, cancellationToken);
        return CreatedAtAction(nameof(Get), new { productId = created.ProductId }, created);
    }

    [HttpDelete("{productId:guid}")]
    [ProducesResponseType(statusCode:StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails),statusCode:StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Delete(Guid productId, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new DeleteProductCommand(productId), cancellationToken);
        return NoContent();
    }
}
