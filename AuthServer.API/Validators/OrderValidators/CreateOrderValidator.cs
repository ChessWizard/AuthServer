using AuthServer.API.Extensions;
using AuthServer.Core.Dtos;
using FluentValidation;

namespace AuthServer.API.Validators.OrderValidators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.RequestedProducts).Required();
            RuleForEach(x => x.RequestedProducts).SetValidator(new RequestedProductValidator());
        }
    }
}
