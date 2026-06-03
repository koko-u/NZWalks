using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NZWalks.Core.Dto;
using NZWalks.Core.Models;
using NZWalks.Core.Tx;

namespace NZWalks.Core.Repositories;

public interface IWalksRepository
{
    Func<DbSession, CancellationToken, Task<IEnumerable<Walk>>> GetAllWalksAsync();

    Func<DbSession, CancellationToken, Task<Walk?>> GetWalkByIdAsync(Guid id);

    Func<DbSession, CancellationToken, Task<Walk>> CreateWalkAsync(NewWalkDto walkDto);
}
