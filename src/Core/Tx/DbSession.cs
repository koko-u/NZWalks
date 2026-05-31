using Npgsql;

namespace NZWalks.Core.Tx;

public readonly record struct DbSession(NpgsqlConnection Conn, NpgsqlTransaction Tx);
