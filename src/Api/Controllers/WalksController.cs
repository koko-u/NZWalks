using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Api.Dto;
using NZWalks.Core.Models;
using NZWalks.Core.Services;

namespace NZWalk.Api.Controllers;

[ApiController]
[Route("api/walks")]
public sealed class WalksController(WalksService walksService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WalkDto>>> GetAll(CancellationToken ct)
    {
        var walks = await walksService.GetAllWalksAsync(ct);
        return Ok(walks.Select(w => w.MapToDto()));
    }
}
