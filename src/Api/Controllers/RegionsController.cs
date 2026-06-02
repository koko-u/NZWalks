using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Api.Extensions;
using NZWalks.Core.Dto;
using NZWalks.Core.Models;
using NZWalks.Core.Services;

namespace NZWalk.Api.Controllers;

[ApiController]
[Route("api/regions")]
public sealed class RegionsController(RegionsService regionsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Region>>> GetAllRegions(CancellationToken ct)
    {
        var regions = await regionsService.GetAllRegionsAsync(ct);
        return Ok(regions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Region>> GetRegionById(Guid id, CancellationToken ct)
    {
        var region = await regionsService.GetRegionByIdAsync(id, ct);
        if (region is null)
        {
            return Problem(
                title: "Region not found",
                detail: $"Region with ID '{id}' was not found",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return Ok(region);
    }

    [HttpPost]
    public async Task<ActionResult> CreateRegion(
        NewRegionDto regionDto,
        [FromServices] IValidator<NewRegionDto> validator,
        CancellationToken ct
    )
    {
        var result = await validator.ValidateAsync(regionDto, ct);
        if (!result.IsValid)
        {
            ModelState.Apply(result.Errors);
            return ValidationProblem(ModelState);
        }

        var region = await regionsService.CreateRegionAsync(regionDto, ct);
        return CreatedAtAction(nameof(GetRegionById), new { id = region.Id }, region);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Region>> UpdateRegion(
        Guid id,
        UpdateRegionDto regionDto,
        [FromServices] IValidator<UpdateRegionDto> validator,
        CancellationToken ct
    )
    {
        var result = await validator.ValidateAsync(regionDto, ct);
        if (!result.IsValid)
        {
            ModelState.Apply(result.Errors);
            return ValidationProblem(ModelState);
        }

        var updated = await regionsService.UpdateRegionAsync(id, regionDto, ct);
        if (updated is null)
        {
            return Problem(
                title: "Region update failed",
                detail: $"Failed to update region with ID '{id}'",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteRegion(Guid id, CancellationToken ct)
    {
        var deleted = await regionsService.DeleteRegionAsync(id, ct);
        if (deleted is null)
        {
            return Problem(
                title: "Region deletion failed",
                detail: $"Failed to delete region with ID '{id}'",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return NoContent();
    }
}
