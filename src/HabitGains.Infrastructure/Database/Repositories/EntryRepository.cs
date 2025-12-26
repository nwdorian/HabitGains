using System.Data.Common;
using HabitGains.Application.Abstractions;
using HabitGains.Domain.Entries;
using HabitGains.Infrastructure.Database.ConnectionFactory;
using Microsoft.Data.Sqlite;

namespace HabitGains.Infrastructure.Database.Repositories;

public class EntryRepository(IDbConnectionFactory connectionFactory) : IEntryRepository
{
    public async Task BulkInsert(List<Entry> entries)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync();
        using DbTransaction transaction = await connection.BeginTransactionAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = """
                INSERT INTO entry (Id, HabitId, Date, Quantity)
                VALUES (@Id, @HabitId, @Date, @Quantity)
            """;

        foreach (Entry entry in entries)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Id", entry.Id);
            command.Parameters.AddWithValue("@HabitId", entry.HabitId);
            command.Parameters.AddWithValue("@Date", entry.Date);
            command.Parameters.AddWithValue("@Quantity", entry.Quantity);

            await command.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
    }

    public async Task<bool> Any()
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync();

        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = "SELECT 1 FROM entry LIMIT 1;";

        using SqliteDataReader reader = await command.ExecuteReaderAsync();

        return reader.HasRows;
    }
}
