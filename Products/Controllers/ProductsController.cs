using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Core.Commands.CreateProduct;
using Products.Core.Commands.DeleteProduct;
using Products.Core.Commands.UpdateProduct;
using Products.Core.Dtos;
using Products.Core.Queries.GetProductById;
using Products.Core.Queries.GetProductBySearch;
using Products.Core.Queries.GetProducts;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Products.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator, ILogger<ProductsController> logger) : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProductsResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(
    typeof(ValidationProblemDetails),
    StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<ProductsResponseDto>>> Get([FromServices]IValidator<GetProductsQuery> validator,int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetProductsQuery(pageNumber, pageSize);
        var validationError = await ValidateRequestAsync(validator, query, cancellationToken);
        if (validationError is not null) return validationError;
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
    public async Task<ActionResult<PagedResult<ProductsResponseDto>>> Search([FromServices] IValidator<GetProductBySearchQuery> validator, [FromQuery]string searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetProductBySearchQuery(searchTerm, pageNumber, pageSize);
        var validationError = await ValidateRequestAsync(validator, query, cancellationToken);
        if (validationError is not null) return validationError;
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

    [HttpPut("{productId:guid}")]
    [ProducesResponseType(typeof(ProductsResponseDto),statusCode:StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status404NotFound)]

    public async Task<ActionResult<ProductsResponseDto>> Update(Guid productId, UpdateProductCommand updateProductCommand, CancellationToken cancellationToken = default)
    {
        updateProductCommand.ProductId = productId;
        return Ok(await mediator.Send(updateProductCommand, cancellationToken));   
    }

    protected async Task<ActionResult?> ValidateRequestAsync<T>(IValidator<T> validator,T request, CancellationToken cancellationToken = default)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (validation.IsValid) return null;
        foreach(var error in validation.Errors)
        {
            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return ValidationProblem(ModelState);
    }
}
