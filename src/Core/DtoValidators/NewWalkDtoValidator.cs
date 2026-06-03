using System;
using FluentValidation;
using NZWalks.Core.Dto;
using NZWalks.Core.Services;

namespace NZWalks.Core.DtoValidators;

public sealed class NewWalkDtoValidator : AbstractValidator<NewWalkDto>
{
    public NewWalkDtoValidator(
        RegionsService regionsService,
        DifficultiesService difficultiesService
    )
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);

        RuleFor(x => x.Description).MaximumLength(4000);

        RuleFor(x => x.LengthKm).NotNull().GreaterThan(0);

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

        RuleFor(x => x.RegionCode)
            .NotEmpty()
            .MustAsync(
                async (regionCode, ct) =>
                {
                    if (regionCode is null)
                    {
                        return true;
                    }

                    var region = await regionsService.GetRegionByCodeAsync(regionCode, ct);

                    return region is not null;
                }
            )
            .WithMessage("Region code is not exists");

        RuleFor(x => x.Difficulty)
            .NotEmpty()
            .MustAsync(
                async (difficulty, ct) =>
                {
                    if (difficulty is null)
                    {
                        return true;
                    }

                    var data = await difficultiesService.GetRegionByNameAsync(difficulty, ct);

                    return data is not null;
                }
            )
            .WithMessage("Difficulty is not exists");
    }
}
