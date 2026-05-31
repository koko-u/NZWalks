using System;
using System.Threading;
using System.Threading.Tasks;
using AutoRegisterAnnotation;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace NZWalks.Core.Tx;

[AutoRegisterService]
public sealed class TxRunner(NpgsqlDataSource dataSource, ILogger<TxRunner> logger)
{
    public async Task<T> ExecuteAsync<T>(
        Func<DbSession, CancellationToken, Task<T>> action,
        CancellationToken ctn
    )
    {
        await using var conn = await dataSource.OpenConnectionAsync(ctn);
        await using var tx = await conn.BeginTransactionAsync(ctn);

        try
        {
            var result = await action(new DbSession(conn, tx), ctn);
            await tx.CommitAsync(ctn);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected Error has occurred.");
            await tx.RollbackAsync(ctn);
            throw;
        }
    }
}
