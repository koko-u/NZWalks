using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using NZWalks.Core.Features.Difficulties.Models;
using NZWalks.Core.Features.Difficulties.Repositories;
using NZWalks.Core.Shared.Tx;

namespace NZWalks.Core.Features.Difficulties.Services;

[AutoRegisterService]
public sealed class DifficultiesService(TxRunner txRunner, IDifficultiesRepository difficultiesRepo)
{
    public Task<Difficulty?> GetRegionByNameAsync(string name, CancellationToken ct) =>
        txRunner.ExecuteAsync(difficultiesRepo.GetRegionByNameAsync(name), ct);
}
