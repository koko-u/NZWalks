using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NZWalks.Core.Dto;
using NZWalks.Core.Models;
using NZWalks.Core.QueryParameters;
using NZWalks.Core.Tx;

namespace NZWalks.Core.Repositories;

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
