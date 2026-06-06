using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Core.Dto;

public sealed class WalkOrder
{
    [FromQuery(Name = "order_by")]
    public string? OrderBy { get; set; }
}
