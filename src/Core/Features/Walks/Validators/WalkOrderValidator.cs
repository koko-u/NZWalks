using System;
using System.Linq;
using FluentValidation;
using NZWalks.Core.Features.Walks.Models;
using NZWalks.Core.Features.Walks.QueryParameters;

namespace NZWalks.Core.Features.Walks.Validators;

public sealed class WalkOrderValidator : AbstractValidator<WalkOrder>
{
    public WalkOrderValidator()
    {
        RuleFor(x => x.OrderBy)
            .Must(orderBy =>
            {
                if (orderBy is null)
                {
                    return true;
                }

                foreach (
                    var entry in orderBy.Split(
                        ",",
                        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                    )
                )
                {
                    var key = entry.StartsWith('-') ? entry[1..] : entry;

                    if (!OrderKey.TryFromValue(key, out _))
                    {
                        return false;
                    }
                }

                return true;
            })
            .WithMessage(
                $"Invalid order by key, which allows one of '{string.Join(",", OrderKey.List.Select(x => x.Value))}'"
            );
    }
}
