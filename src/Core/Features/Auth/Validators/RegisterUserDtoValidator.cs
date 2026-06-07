using System;
using System.Collections.Generic;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NZWalks.Core.Features.Auth.Dto;

namespace NZWalks.Core.Features.Auth.Validators;

public sealed class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(ILogger<RegisterUserDtoValidator> logger)
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.DisplayName).MaximumLength(255);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Custom(
                (password, context) =>
                {
                    if (password is null)
                    {
                        return;
                    }

                    var email = context.InstanceToValidate.Email;
                    var displayName = context.InstanceToValidate.DisplayName;

                    var userInput = new List<string>();
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        userInput.AddRange(email.Split('@', '.'));
                    }
                    if (!string.IsNullOrWhiteSpace(displayName))
                    {
                        userInput.AddRange(
                            displayName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        );
                    }

                    var result = Zxcvbn.Core.EvaluatePassword(password, userInput);
                    if (result.Score < 3)
                    {
                        logger.LogInformation(
                            $"Password '{password}' has not have enough complexity. Score: {result.Score}"
                        );

                        var warning = string.IsNullOrEmpty(result.Feedback.Warning)
                            ? "Provided Password is too weak."
                            : result.Feedback.Warning!;
                        context.AddFailure("password", warning);
                        foreach (var suggestion in result.Feedback.Suggestions)
                        {
                            if (!string.IsNullOrEmpty(suggestion))
                            {
                                context.AddFailure("password", suggestion);
                            }
                        }
                    }
                }
            );
    }
}
