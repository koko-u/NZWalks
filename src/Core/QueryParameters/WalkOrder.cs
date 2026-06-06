using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Core.QueryParameters;

public sealed class WalkOrder
{
    [FromQuery(Name = "order_by")]
    public string? OrderBy { get; set; }
}
