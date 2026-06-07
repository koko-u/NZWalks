using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using NZWalks.Core.Features.Walks.Dto;
using NZWalks.Core.Features.Walks.Models;
using NZWalks.Core.Features.Walks.QueryParameters;
using NZWalks.Core.Features.Walks.Repositories;
using NZWalks.Core.Shared.Paging;
using NZWalks.Core.Shared.Tx;

namespace NZWalks.Core.Features.Walks.Services;

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
