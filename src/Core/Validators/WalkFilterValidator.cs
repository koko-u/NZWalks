using FluentValidation;
using NZWalks.Core.QueryParameters;
using NZWalks.Core.Services;

namespace NZWalks.Core.Validators;

public sealed class WalkFilterValidator : AbstractValidator<WalkFilter>
{
    public WalkFilterValidator(
        RegionsService regionsService,
        DifficultiesService difficultiesService
    )
    {
        RuleFor(x => x.NameLike).MaximumLength(255);

        RuleFor(x => x.MinLength).GreaterThanOrEqualTo(0.0);

        RuleFor(x => x.MaxLength).GreaterThanOrEqualTo(x => x.MinLength ?? 0.0);

        RuleFor(x => x.RegionCode)
            .MustAsync(
                async (code, ct) =>
                {
                    if (code is null)
                    {
                        return true;
                    }

                    var region = await regionsService.GetRegionByCodeAsync(code, ct);

                    return region is not null;
                }
            )
            .WithMessage("Region code is invalid");

        RuleFor(x => x.Difficulty)
            .MustAsync(
                async (difficulty, ct) =>
                {
                    if (difficulty is null)
                    {
                        return true;
                    }

                    var row = await difficultiesService.GetRegionByNameAsync(difficulty, ct);
                    return row is not null;
                }
            )
            .WithMessage("Difficulty is invalid");
    }
}
