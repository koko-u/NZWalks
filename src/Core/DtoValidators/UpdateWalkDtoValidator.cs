using System;
using FluentValidation;
using NZWalks.Core.Dto;
using NZWalks.Core.Services;

namespace NZWalks.Core.DtoValidators;

public sealed class UpdateWalkDtoValidator : AbstractValidator<UpdateWalkDto>
{
    public UpdateWalkDtoValidator(
        RegionsService regionsService,
        DifficultiesService difficultiesService
    )
    {
        RuleFor(x => x.Name).MaximumLength(255);

        RuleFor(x => x.Description).MaximumLength(4000);

        RuleFor(x => x.LengthKm).GreaterThan(0);

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

        RuleFor(x => x)
            .Must(dto =>
                dto.Name is not null
                || dto.Description is not null
                || dto.LengthKm is not null
                || dto.ImageUrl is not null
                || dto.RegionCode is not null
                || dto.Difficulty is not null
            )
            .WithMessage("At least one field must be provided for update");
    }
}
