

using FluentValidation;
using Products.Core.Queries.GetProducts;

namespace Products.Core.Validators;
public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(p => p.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0");
        RuleFor(p => p.PageSize).GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size cant be greater than 100");
    }
}
