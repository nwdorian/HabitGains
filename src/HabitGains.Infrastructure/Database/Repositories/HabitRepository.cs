using System.Data.Common;
using HabitGains.Application.Abstractions;
using HabitGains.Domain.Habits;
using HabitGains.Infrastructure.Database.ConnectionFactory;
using Microsoft.Data.Sqlite;

namespace HabitGains.Infrastructure.Database.Repositories;

public class HabitRepository(IDbConnectionFactory connectionFactory) : IHabitRepository
{
    public async Task BulkInsert(List<Habit> habits)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync();
        using DbTransaction transaction = await connection.BeginTransactionAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = """
                INSERT INTO habit (Id, Name, Measurement, Favorite)
                VALUES (@Id, @Name, @Measurement, @Favorite)
            """;

        foreach (Habit habit in habits)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Id", habit.Id);
            command.Parameters.AddWithValue("@Name", habit.Name);
            command.Parameters.AddWithValue("@Measurement", habit.Measurement);
            command.Parameters.AddWithValue("@Favorite", habit.Favorite);

            await command.ExecuteNonQueryAsync();
        }

        await transaction.CommitAsync();
    }

    public async Task<bool> Any()
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync();

        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = "SELECT 1 FROM habit LIMIT 1;";

        using SqliteDataReader reader = await command.ExecuteReaderAsync();

        return reader.HasRows;
    }
}
