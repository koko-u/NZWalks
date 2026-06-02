using NZWalk.Infrastructure.Rows;
using NZWalks.Core.Models;
using Riok.Mapperly.Abstractions;

namespace NZWalk.Infrastructure.Mappers;

[Mapper(ThrowOnPropertyMappingNullMismatch = true)]
public static partial class RegionRowMapper
{
    public static partial Region ToRegionModel(this RegionRow row);
}
