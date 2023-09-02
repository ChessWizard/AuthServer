using AuthServer.API.Extensions;
using AuthServer.Core.Dtos;
using FluentValidation;

namespace AuthServer.API.Validators.OrderValidators
{
    public class RequestedProductValidator : AbstractValidator<RequestedProduct>
    {
        public RequestedProductValidator()
        {
            RuleFor(x => x.ProductId).Required();
            RuleFor(x => x.Count).Required();
        }
    }
}
