using AuthServer.API.Extensions;
using AuthServer.Core.Dtos;
using FluentValidation;

namespace AuthServer.API.Validators.ProductValidators
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).Required();
            RuleFor(x => x.Name).MaximumLength(200);
            RuleFor(x => x.Price).Required();
            RuleFor(x => x.Stock).Required();
        }
    }
}
