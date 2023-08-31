using AuthServer.API.Extensions;
using AuthServer.Core.Dtos;
using FluentValidation;

namespace AuthServer.API.Validators.UserValidators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName).Required();
            RuleFor(x => x.UserName).MaximumLength(20);
            RuleFor(x => x.Email).Required();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Required();
        }
    }
}
