using System;
using System.Collections.Generic;

namespace NZWalk.Api.Responses;

public sealed class MeResponse
{
    public required Guid UserId { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public List<string> Roles { get; set; } = [];
}
