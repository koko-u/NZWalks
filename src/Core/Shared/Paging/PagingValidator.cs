using FluentValidation;

namespace NZWalks.Core.Shared.Paging;

public sealed class PagingValidator : AbstractValidator<Paging>
{
    public PagingValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
    }
}
