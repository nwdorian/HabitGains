using System.Data;
using Microsoft.Data.Sqlite;

namespace HabitGains.Infrastructure.Database.ConnectionFactory;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        SqliteConnection connection = new(connectionString);
        await connection.OpenAsync(token);
        return connection;
    }
}
