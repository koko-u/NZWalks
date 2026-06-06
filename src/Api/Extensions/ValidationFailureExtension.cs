using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NZWalk.Api.Extensions;

public static class ValidationFailureExtension
{
    public static ModelStateDictionary Apply(
        this ModelStateDictionary modelState,
        List<ValidationFailure> errors
    )
    {
        foreach (var failure in errors)
        {
            modelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
        }

        return modelState;
    }
}
