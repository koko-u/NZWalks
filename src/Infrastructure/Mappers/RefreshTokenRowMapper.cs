using NZWalk.Infrastructure.Rows;
using NZWalks.Core.Features.Auth.Models;
using Riok.Mapperly.Abstractions;

namespace NZWalk.Infrastructure.Mappers;

[Mapper(ThrowOnPropertyMappingNullMismatch = true)]
public static partial class RefreshTokenRowMapper
{
    public static partial RefreshToken ToRefreshTokenModel(this RefreshTokenRow row);
}
