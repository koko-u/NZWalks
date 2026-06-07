using System;
using System.Threading;
using System.Threading.Tasks;
using NZWalks.Core.Features.Auth.Models;
using NZWalks.Core.Shared.Tx;

namespace NZWalks.Core.Features.Auth.Repositories;

public interface IRefreshTokensRepository
{
    Func<DbSession, CancellationToken, Task> ExpireAsync(Guid userId);

    Func<DbSession, CancellationToken, Task> CreateAsync(Guid userId, string hashedToken);

    Func<DbSession, CancellationToken, Task> RevokeAsync(string hashedToken);

    Func<DbSession, CancellationToken, Task<RefreshToken?>> GetByTokenAsync(string hashedToken);
}
