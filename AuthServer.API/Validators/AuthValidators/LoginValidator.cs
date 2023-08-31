using AuthServer.API.Extensions;
using AuthServer.Core.Dtos;
using FluentValidation;

namespace AuthServer.API.Validators.AuthValidators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Required();
        }
    }
}
