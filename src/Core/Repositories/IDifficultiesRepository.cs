using System;
using System.Threading;
using System.Threading.Tasks;
using NZWalks.Core.Models;
using NZWalks.Core.Tx;

namespace NZWalks.Core.Repositories;

public interface IDifficultiesRepository
{
    Func<DbSession, CancellationToken, Task<Difficulty?>> GetRegionByNameAsync(string name);
}
