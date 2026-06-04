using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using NZWalks.Core.Dto;
using NZWalks.Core.Models;
using NZWalks.Core.Repositories;
using NZWalks.Core.Tx;

namespace NZWalks.Core.Services;

[AutoRegisterService]
public sealed class WalksService(TxRunner txRunner, IWalksRepository walksRepo)
{
    public async Task<IEnumerable<Walk>> GetAllWalksAsync(CancellationToken ct) =>
        await txRunner.ExecuteAsync(walksRepo.GetAllWalksAsync(), ct);

    public async Task<Walk?> GetWalkByIdAsync(Guid id, CancellationToken ct) =>
        await txRunner.ExecuteAsync(walksRepo.GetWalkByIdAsync(id), ct);

    public async Task<Walk> CreateWalkAsync(NewWalkDto walkDto, CancellationToken ct) =>
        await txRunner.ExecuteAsync(walksRepo.CreateWalkAsync(walkDto), ct);

    public async Task<Walk?> UpdateWalkAsync(
        Guid id,
        UpdateWalkDto walkDto,
        CancellationToken ct
    ) => await txRunner.ExecuteAsync(walksRepo.UpdateWalkAsync(id, walkDto), ct);
}
