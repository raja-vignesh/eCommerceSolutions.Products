
using FluentValidation;
using Products.Core.Queries.GetProductBySearch;

namespace Products.Core.Validators;
public class GetProductsBySearchValidator : AbstractValidator<GetProductBySearchQuery>
{
    public GetProductsBySearchValidator()
    {
        RuleFor(p => p.SearchTerm).NotEmpty().WithMessage("Please enter search text");
        RuleFor(p => p.PageNumber).GreaterThan(0).WithMessage("Page number should be greater than 0");
        RuleFor(p => p.PageSize).InclusiveBetween(1,100).WithMessage("Page size must be between 1 and 100");
    }
}
