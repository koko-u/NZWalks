using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NZWalks.Core.Features.Walks.Dto;
using NZWalks.Core.Features.Walks.Models;
using NZWalks.Core.Features.Walks.QueryParameters;
using NZWalks.Core.Shared.Paging;
using NZWalks.Core.Shared.Tx;

namespace NZWalks.Core.Features.Walks.Repositories;

public interface IWalksRepository
{
    Func<DbSession, CancellationToken, Task<IEnumerable<Walk>>> GetWalksAsync(
        WalkFilter filter,
        WalkOrder order,
        Paging paging
    );

    Func<DbSession, CancellationToken, Task<int>> GetWalksTotalCount(WalkFilter filter);

    Func<DbSession, CancellationToken, Task<Walk?>> GetWalkByIdAsync(Guid id);

    Func<DbSession, CancellationToken, Task<Walk>> CreateWalkAsync(NewWalkDto walkDto);

    Func<DbSession, CancellationToken, Task<Walk?>> UpdateWalkAsync(Guid id, UpdateWalkDto walkDto);

    Func<DbSession, CancellationToken, Task<Walk?>> DeleteWalkAsync(Guid id);
}
