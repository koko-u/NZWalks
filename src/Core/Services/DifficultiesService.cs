using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using NZWalks.Core.Models;
using NZWalks.Core.Repositories;
using NZWalks.Core.Tx;

namespace NZWalks.Core.Services;

[AutoRegisterService]
public sealed class DifficultiesService(TxRunner txRunner, IDifficultiesRepository difficultiesRepo)
{
    public Task<Difficulty?> GetRegionByNameAsync(string name, CancellationToken ct) =>
        txRunner.ExecuteAsync(difficultiesRepo.GetRegionByNameAsync(name), ct);
}
