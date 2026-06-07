using System;
using System.Threading;
using System.Threading.Tasks;
using NZWalks.Core.Features.Difficulties.Models;
using NZWalks.Core.Shared.Tx;

namespace NZWalks.Core.Features.Difficulties.Repositories;

public interface IDifficultiesRepository
{
    Func<DbSession, CancellationToken, Task<Difficulty?>> GetRegionByNameAsync(string name);
}
