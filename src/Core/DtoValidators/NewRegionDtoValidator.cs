using System;
using FluentValidation;
using NZWalks.Core.Dto;
using NZWalks.Core.Services;

namespace NZWalks.Core.DtoValidators;

public sealed class NewRegionDtoValidator : AbstractValidator<NewRegionDto>
{
    public NewRegionDtoValidator(RegionsService regionsService)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(255)
            .MustAsync(
                async (code, ct) =>
                {
                    var existsRegion = await regionsService.GetRegionByCodeAsync(code, ct);
                    return existsRegion == null;
                }
            )
            .WithMessage("Region with the same code = {PropertyValue} already exists");

        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);

        RuleFor(x => x.ImageUrl)
            .MaximumLength(2048)
            .Must(url =>
            {
                if (url is null)
                    return true;

                if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
                {
                    return false;
                }

                return uriResult.Scheme == Uri.UriSchemeHttp
                    || uriResult.Scheme == Uri.UriSchemeHttps;
            })
            .WithMessage("Invalid image URL format");
    }
}
