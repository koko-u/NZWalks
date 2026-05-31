using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using NZWalks.Core.Models;
using NZWalks.Core.Repositories;
using NZWalks.Core.Tx;

namespace NZWalks.Core.Services;

[AutoRegisterService]
public sealed class WalksService(TxRunner txRunner, IWalksRepository walksRepo)
{
    public async Task<IEnumerable<Walk>> GetAllWalksAsync(CancellationToken ct)
    {
        return await txRunner.ExecuteAsync(walksRepo.GetAllWalksAsync, ct);
    }
}
