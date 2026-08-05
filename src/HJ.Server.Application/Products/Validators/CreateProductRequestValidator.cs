using FluentValidation;
using HJ.Server.Contracts.Products.Requests;

namespace HJ.Server.Application.Products.Validators;

public class CreateProductRequestValidator 
    : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
