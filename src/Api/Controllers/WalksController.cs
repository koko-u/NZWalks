using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Core.Dto;
using NZWalks.Core.Services;
using WalkDtoMapper = NZWalks.Core.Mappers.WalkDtoMapper;

namespace NZWalk.Api.Controllers;

[ApiController]
[Route("api/walks")]
public sealed class WalksController(WalksService walksService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WalkDto>>> GetAll(CancellationToken ct)
    {
        var walks = await walksService.GetAllWalksAsync(ct);
        return Ok(walks.Select(WalkDtoMapper.MapToDto));
    }
}
