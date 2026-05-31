using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NZWalks.Core.Models;
using NZWalks.Core.Tx;

namespace NZWalks.Core.Repositories;

public interface IWalksRepository
{
    Task<IEnumerable<Walk>> GetAllWalksAsync(DbSession session, CancellationToken ct);
}
