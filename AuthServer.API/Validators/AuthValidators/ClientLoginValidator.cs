using AuthServer.API.Extensions;
using AuthServer.Core.Dtos;
using FluentValidation;

namespace AuthServer.API.Validators.AuthValidators
{
    public class ClientLoginValidator : AbstractValidator<ClientLoginDto>
    {
        public ClientLoginValidator()
        {
            RuleFor(x => x.ClientId).Required();
            RuleFor(x => x.ClientSecret).Required();
        }
    }
}
