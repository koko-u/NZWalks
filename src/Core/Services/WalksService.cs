using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using NZWalks.Core.Data;
using NZWalks.Core.Dto;
using NZWalks.Core.Models;
using NZWalks.Core.QueryParameters;
using NZWalks.Core.Repositories;
using NZWalks.Core.Responses;
using NZWalks.Core.Tx;

namespace NZWalks.Core.Services;

[AutoRegisterService]
public sealed class WalksService(TxRunner txRunner, IWalksRepository walksRepo)
{
    public async Task<PagingItems<Walk>> GetWalksAsync(
        WalkFilter filter,
        WalkOrder order,
        Paging paging,
        CancellationToken ct
    ) =>
        await txRunner.ExecuteAsync(
            async (session, ctn) =>
            {
                var rows = await walksRepo.GetWalksAsync(filter, order, paging)(session, ctn);
                var count = await walksRepo.GetWalksTotalCount(filter)(session, ctn);

                return new PagingItems<Walk>()
                {
                    Items = rows.ToList(),
                    Page = new PageInfo(paging.PageNumber, paging.PageSize, count),
                };
            },
            ct
        );

    public async Task<Walk?> GetWalkByIdAsync(Guid id, CancellationToken ct) =>
        await txRunner.ExecuteAsync(walksRepo.GetWalkByIdAsync(id), ct);

    public async Task<Walk> CreateWalkAsync(NewWalkDto walkDto, CancellationToken ct) =>
        await txRunner.ExecuteAsync(walksRepo.CreateWalkAsync(walkDto), ct);

    public async Task<Walk?> UpdateWalkAsync(
        Guid id,
        UpdateWalkDto walkDto,
        CancellationToken ct
    ) => await txRunner.ExecuteAsync(walksRepo.UpdateWalkAsync(id, walkDto), ct);

    public async Task<Walk?> DeleteWalkAsync(Guid id, CancellationToken ct) =>
        await txRunner.ExecuteAsync(walksRepo.DeleteWalkAsync(id), ct);
}
