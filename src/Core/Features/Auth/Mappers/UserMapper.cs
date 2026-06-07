using NZWalks.Core.Features.Auth.Models;
using Riok.Mapperly.Abstractions;

namespace NZWalks.Core.Features.Auth.Mappers;

[Mapper(ThrowOnPropertyMappingNullMismatch = true)]
public static partial class UserMapper
{
    [MapperIgnoreSource(nameof(UserCredentials.PasswordHash))]
    public static partial User ToUser(this UserCredentials userCredentials);
}
