
using FluentValidation;
using Products.Core.Commands.CreateProduct;
using Products.Core.enums;

namespace Products.Core.Validators;
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.ProductName).NotEmpty().WithMessage("Product Name is required").MaximumLength(50)
            .WithMessage("Product name should not exceed 50 characters.");
        RuleFor(p => p.UnitPrice).GreaterThan(0).WithMessage("price should be greater than 0");
        RuleFor(p => p.QuantityInStock).GreaterThan(0).WithMessage("Qty should be greater than 0");
        RuleFor(p => p.Category).NotNull().WithMessage("Category is required").IsInEnum().WithMessage("Invalid prod category");
    }
}
