using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Core.Features.Walks.QueryParameters;

public sealed class WalkOrder
{
    [FromQuery(Name = "order_by")]
    public string? OrderBy { get; set; }
}
