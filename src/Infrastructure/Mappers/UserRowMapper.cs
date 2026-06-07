using NZWalk.Infrastructure.Rows;
using NZWalks.Core.Features.Auth.Models;
using Riok.Mapperly.Abstractions;

namespace NZWalk.Infrastructure.Mappers;

[Mapper(ThrowOnPropertyMappingNullMismatch = true)]
public static partial class UserRowMapper
{
    public static partial User ToUserModel(this UserRow row);
}
