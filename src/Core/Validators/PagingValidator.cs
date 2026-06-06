using FluentValidation;
using NZWalks.Core.QueryParameters;

namespace NZWalks.Core.Validators;

public sealed class PagingValidator : AbstractValidator<Paging>
{
    public PagingValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
    }
}
