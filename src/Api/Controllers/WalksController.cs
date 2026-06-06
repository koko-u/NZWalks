using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalk.Api.Extensions;
using NZWalks.Core.Dto;
using NZWalks.Core.Mappers;
using NZWalks.Core.Services;

namespace NZWalk.Api.Controllers;

[ApiController]
[Route("api/walks")]
public sealed class WalksController(WalksService walksService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<WalkDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WalkDto>>> GetAll(
        [FromQuery] WalkFilter filter,
        [FromServices] IValidator<WalkFilter> validator,
        [FromQuery] WalkOrder order,
        [FromServices] IValidator<WalkOrder> orderValidator,
        CancellationToken ct
    )
    {
        var filterResult = await validator.ValidateAsync(filter, ct);
        var orderResult = await orderValidator.ValidateAsync(order, ct);
        if (!(filterResult.IsValid && orderResult.IsValid))
        {
            ModelState.Apply(filterResult.Errors);
            ModelState.Apply(orderResult.Errors);
            return ValidationProblem(ModelState);
        }

        var walks = await walksService.GetAllWalksAsync(filter, order, ct);
        return Ok(walks.Select(walk => walk.MapToDto()));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<WalkDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalkDto>> GetById(Guid id, CancellationToken ct)
    {
        var walk = await walksService.GetWalkByIdAsync(id, ct);
        if (walk is null)
        {
            return Problem(
                title: "Walk not found",
                detail: $"Walk with ID '{id}' does not exist",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return Ok(walk.MapToDto());
    }

    [HttpPost]
    [ProducesResponseType<WalkDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WalkDto>> Create(
        [FromBody] NewWalkDto walkDto,
        [FromServices] IValidator<NewWalkDto> validator,
        CancellationToken ct
    )
    {
        var result = await validator.ValidateAsync(walkDto, ct);
        if (!result.IsValid)
        {
            ModelState.Apply(result.Errors);
            return ValidationProblem(ModelState);
        }

        var created = await walksService.CreateWalkAsync(walkDto, ct);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.MapToDto());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<WalkDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalkDto>> Update(
        Guid id,
        [FromBody] UpdateWalkDto walkDto,
        [FromServices] IValidator<UpdateWalkDto> validator,
        CancellationToken ct
    )
    {
        var result = await validator.ValidateAsync(walkDto, ct);
        if (!result.IsValid)
        {
            ModelState.Apply(result.Errors);
            return ValidationProblem(ModelState);
        }

        var updated = await walksService.UpdateWalkAsync(id, walkDto, ct);
        if (updated == null)
        {
            return Problem(
                title: "Walk not found",
                detail: "The requested walk could not be found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return Ok(updated.MapToDto());
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await walksService.DeleteWalkAsync(id, ct);
        if (deleted is null)
        {
            return Problem(
                title: "Walk not found",
                detail: "The requested walk could not be found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return NoContent();
    }
}
