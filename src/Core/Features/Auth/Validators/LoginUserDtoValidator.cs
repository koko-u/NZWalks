using FluentValidation;
using NZWalks.Core.Features.Auth.Dto;

namespace NZWalks.Core.Features.Auth.Validators;

public sealed class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Password).NotEmpty();
    }
}
