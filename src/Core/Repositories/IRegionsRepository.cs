using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NZWalks.Core.Dto;
using NZWalks.Core.Models;
using NZWalks.Core.Tx;

namespace NZWalks.Core.Repositories;

public interface IRegionsRepository
{
    Func<DbSession, CancellationToken, Task<IEnumerable<Region>>> GetAllRegionsAsync();

    Func<DbSession, CancellationToken, Task<Region?>> GetRegionByIdAsync(Guid id);

    Func<DbSession, CancellationToken, Task<Region?>> GetRegionByCodeAsync(string code);

    Func<DbSession, CancellationToken, Task<Region>> CreateRegionAsync(NewRegionDto newRegionDto);

    Func<DbSession, CancellationToken, Task<Region?>> UpdateRegionAsync(
        Guid id,
        UpdateRegionDto updateRegionDto
    );

    Func<DbSession, CancellationToken, Task<Region?>> DeleteRegionAsync(Guid id);
}
