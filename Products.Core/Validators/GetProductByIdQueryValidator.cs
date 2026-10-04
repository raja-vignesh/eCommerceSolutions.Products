
using FluentValidation;
using Products.Core.Queries.GetProductById;

namespace Products.Core.Validators;
public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(p => p.ProudctId).NotEmpty().WithMessage("Product id is required");            
                
    }
}
