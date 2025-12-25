using System.Data;
using HabitGains.Infrastructure.Database.ConnectionFactory;

namespace HabitGains.Infrastructure.Database.Initializer;

public class DbInitializer(IDbConnectionFactory connectionFactory) : IDbInitializer
{
    public async Task RunAsync()
    {
        using IDbConnection connection = await connectionFactory.CreateConnectionAsync();

        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            CREATE TABLE IF NOT EXISTS habit (
                Id TEXT PRIMARY KEY,
                Name TEXT NOT NULL,
                Measurement TEXT NOT NULL
                )
            """;

        command.ExecuteNonQuery();

        command.CommandText = """
            CREATE TABLE IF NOT EXISTS habit_entry (
                Id TEXT PRIMARY KEY,
                HabitId TEXT NOT NULL,
                Date TEXT NOT NULL,
                Quantity REAL NOT NULL,
                FOREIGN KEY (HabitId) REFERENCES habit(Id)
            )
            """;

        command.ExecuteNonQuery();
    }
}
