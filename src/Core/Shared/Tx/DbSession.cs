using Npgsql;

namespace NZWalks.Core.Shared.Tx;

public readonly record struct DbSession(NpgsqlConnection Conn, NpgsqlTransaction Tx);
