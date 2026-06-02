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
public sealed class RegionsService(TxRunner txRunner, IRegionsRepository regionsRepo)
{
    public Task<IEnumerable<Region>> GetAllRegionsAsync(CancellationToken ct) =>
        txRunner.ExecuteAsync(regionsRepo.GetAllRegionsAsync(), ct);

    public Task<Region?> GetRegionByIdAsync(Guid id, CancellationToken ct) =>
        txRunner.ExecuteAsync(regionsRepo.GetRegionByIdAsync(id), ct);

    public Task<Region?> GetRegionByCodeAsync(string code, CancellationToken ct) =>
        txRunner.ExecuteAsync(regionsRepo.GetRegionByCodeAsync(code), ct);

    public Task<Region> CreateRegionAsync(NewRegionDto newRegionDto, CancellationToken ct) =>
        txRunner.ExecuteAsync(regionsRepo.CreateRegionAsync(newRegionDto), ct);

    public Task<Region?> UpdateRegionAsync(
        Guid id,
        UpdateRegionDto updateRegionDto,
        CancellationToken ct
    ) => txRunner.ExecuteAsync(regionsRepo.UpdateRegionAsync(id, updateRegionDto), ct);

    public Task<Region?> DeleteRegionAsync(Guid id, CancellationToken ct) =>
        txRunner.ExecuteAsync(regionsRepo.DeleteRegionAsync(id), ct);
}
