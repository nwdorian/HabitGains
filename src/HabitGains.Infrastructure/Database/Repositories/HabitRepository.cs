using System.Data.Common;
using System.Globalization;
using System.Text;
using HabitGains.Application.Core.Abstractions.Repositories;
using HabitGains.Application.Core.Pagination;
using HabitGains.Application.Core.Pagination.Entries;
using HabitGains.Application.Core.Pagination.Habits;
using HabitGains.Domain.Entries;
using HabitGains.Domain.Habits;
using HabitGains.Infrastructure.Database.ConnectionFactory;
using Microsoft.Data.Sqlite;

namespace HabitGains.Infrastructure.Database.Repositories;

public class HabitRepository(IDbConnectionFactory connectionFactory) : IHabitRepository
{
    public async Task<IReadOnlyList<Habit>> GetHabitsPage(
        HabitFilter filter,
        HabitSorting sorting,
        Paging paging,
        CancellationToken cancellationToken
    )
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        StringBuilder query = new(
            """
            SELECT Id, Name, Measurement, Favorite, CreatedAt, UpdatedAt
            FROM habit
            WHERE 1=1
            """
        );

        ApplyFilter(query, command, filter);
        ApplySorting(query, sorting);
        ApplyPaging(query, command, paging);

        command.CommandText = query.ToString();

        using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        List<Habit> habits = new();

        while (await reader.ReadAsync(cancellationToken))
        {
            Habit habit = new()
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Measurement = reader.GetString(2),
                Favorite = reader.GetBoolean(3),
                CreatedAt = reader.GetDateTime(4),
                UpdatedAt = await reader.IsDBNullAsync(5, cancellationToken) ? null : reader.GetDateTime(5),
            };

            habits.Add(habit);
        }

        return habits;
    }

    public async Task<int> Count(HabitFilter filter, CancellationToken cancellationToken)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        StringBuilder query = new(
            """
            SELECT COUNT(*)
            FROM habit
            WHERE 1 = 1
            """
        );

        ApplyFilter(query, command, filter);

        command.CommandText = query.ToString();

        using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        await reader.ReadAsync(cancellationToken);
        return reader.GetInt32(0);
    }

    public async Task<Habit?> GetByIdWithEntriesPage(
        Guid id,
        EntryFilter filter,
        EntrySorting sorting,
        Paging paging,
        CancellationToken cancellationToken
    )
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        StringBuilder query = new(
            """
            SELECT h.Id, h.Name, h.Measurement, h.Favorite, h.CreatedAt, h.UpdatedAt, e.Id, e.HabitId, e.Date, e.Quantity, e.CreatedAt, e.UpdatedAt
            FROM habit AS h
            INNER JOIN Entry AS e
                ON h.Id = e.HabitId
            WHERE h.Id = @Id
            """
        );

        ApplyEntryFilter(query, command, filter);
        ApplyEntrySorting(query, sorting);
        ApplyPaging(query, command, paging);

        command.CommandText = query.ToString();

        using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        Habit habit = new()
        {
            Id = reader.GetGuid(0),
            Name = reader.GetString(1),
            Measurement = reader.GetString(2),
            Favorite = reader.GetBoolean(3),
            CreatedAt = reader.GetDateTime(4),
            UpdatedAt = await reader.IsDBNullAsync(5, cancellationToken) ? null : reader.GetDateTime(5),
        };

        List<Entry> entries = new();

        while (await reader.ReadAsync(cancellationToken))
        {
            Entry entry = new()
            {
                Id = reader.GetGuid(6),
                HabitId = reader.GetGuid(7),
                Date = reader.GetDateTime(8),
                Quantity = reader.GetDecimal(9),
                CreatedAt = reader.GetDateTime(10),
                UpdatedAt = await reader.IsDBNullAsync(5, cancellationToken) ? null : reader.GetDateTime(5),
            };

            entries.Add(entry);
        }

        habit.Entries = entries;

        return habit;
    }

    public async Task<int> CountWithEntries(Guid id, EntryFilter filter, CancellationToken cancellationToken)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        StringBuilder query = new(
            """
            SELECT h.Id, h.Name, h.Measurement, h.Favorite, h.CreatedAt, h.UpdatedAt, e.Id, e.HabitId, e.Date, e.Quantity, e.CreatedAt, e.UpdatedAt
            FROM habit AS h
            INNER JOIN Entry AS e
                ON h.Id = e.HabitId
            WHERE h.Id = @Id
            """
        );

        ApplyEntryFilter(query, command, filter);

        command.CommandText = query.ToString();

        using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        await reader.ReadAsync(cancellationToken);
        return reader.GetInt32(0);
    }

    public async Task<Habit?> GetById(Guid id, CancellationToken cancellationToken)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = """
                SELECT Id, Name, Measurement, Favorite, CreatedAt, UpdatedAt
                FROM habit 
                WHERE Id = @Id
            """;

        command.Parameters.AddWithValue("Id", id);

        using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return new Habit()
        {
            Id = reader.GetGuid(0),
            Name = reader.GetString(1),
            Measurement = reader.GetString(2),
            Favorite = reader.GetBoolean(3),
            CreatedAt = reader.GetDateTime(4),
            UpdatedAt = await reader.IsDBNullAsync(5, cancellationToken) ? null : reader.GetDateTime(5),
        };
    }

    public async Task Create(Habit habit, CancellationToken cancellationToken)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = """
                INSERT INTO habit (Id, Name, Measurement, Favorite, CreatedAt, UpdatedAt)
                VALUES (@Id, @Name, @Measurement, @Favorite, @CreatedAt, @UpdatedAt)
            """;

        command.Parameters.AddWithValue("@Id", habit.Id);
        command.Parameters.AddWithValue("@Name", habit.Name);
        command.Parameters.AddWithValue("@Measurement", habit.Measurement);
        command.Parameters.AddWithValue("@Favorite", habit.Favorite);
        command.Parameters.AddWithValue("@CreatedAt", habit.CreatedAt);
        command.Parameters.AddWithValue("@UpdatedAt", habit.UpdatedAt is null ? DBNull.Value : habit.UpdatedAt);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = "DELETE FROM habit WHERE Id = @Id";

        command.Parameters.AddWithValue("Id", id);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task Update(Habit habit, CancellationToken cancellationToken)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = """
            UPDATE habit
            SET Name = @Name, Measurement = @Measurement, Favorite = @Favorite, UpdatedAt = @UpdatedAt 
            WHERE Id = @Id
            """;

        command.Parameters.AddWithValue("@Id", habit.Id);
        command.Parameters.AddWithValue("@Name", habit.Name);
        command.Parameters.AddWithValue("@Measurement", habit.Measurement);
        command.Parameters.AddWithValue("@Favorite", habit.Favorite);
        command.Parameters.AddWithValue("@UpdatedAt", habit.UpdatedAt);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = """
            SELECT 1
            FROM habit
            WHERE LOWER(Name) = LOWER(@Name)
            LIMIT 1;
            """;

        command.Parameters.AddWithValue("@Name", name);

        using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        return !reader.HasRows;
    }

    public async Task<List<string>> GetHabitMeasurements(CancellationToken cancellationToken)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = """
                SELECT DISTINCT Measurement FROM habit
            """;

        using SqliteDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        List<string> names = new();

        while (await reader.ReadAsync(cancellationToken))
        {
            names.Add(reader.GetString(0));
        }

        return names;
    }

    public async Task BulkInsert(List<Habit> habits)
    {
        using SqliteConnection connection = await connectionFactory.CreateConnectionAsync();
        using DbTransaction transaction = await connection.BeginTransactionAsync();
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = """
                INSERT INTO habit (Id, Name, Measurement, Favorite, CreatedAt, UpdatedAt)
                VALUES (@Id, @Name, @Measurement, @Favorite, @CreatedAt, @UpdatedAt)
            """;

        foreach (Habit habit in habits)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Id", habit.Id);
            command.Parameters.AddWithValue("@Name", habit.Name);
            command.Parameters.AddWithValue("@Measurement", habit.Measurement);
            command.Parameters.AddWithValue("@Favorite", habit.Favorite);
            command.Parameters.AddWithValue("@CreatedAt", habit.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", habit.UpdatedAt is null ? DBNull.Value : habit.UpdatedAt);

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

    private static void ApplyFilter(StringBuilder query, SqliteCommand command, HabitFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Measurement))
        {
            query.Append(" AND Measurement LIKE @Measurement");
            command.Parameters.AddWithValue("@Measurement", filter.Measurement);
        }

        if (filter.Favorite is not null)
        {
            query.Append(" AND Favorite = @Favorite");
            command.Parameters.AddWithValue("@Favorite", filter.Favorite);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query.Append(" AND (Name LIKE @SearchTerm OR Measurement LIKE @SearchTerm)");
            command.Parameters.AddWithValue("@SearchTerm", $"%{filter.SearchTerm.Trim()}%");
        }
    }

    private static void ApplySorting(StringBuilder query, HabitSorting sorting)
    {
        query.Append(CultureInfo.InvariantCulture, $" ORDER BY {sorting.SortColumn} {sorting.SortOrder}");
    }

    private static void ApplyEntryFilter(StringBuilder query, SqliteCommand command, EntryFilter filter)
    {
        if (filter.QuantityFrom is not null)
        {
            query.Append(" AND Quantity > @QuantityFrom");
            command.Parameters.AddWithValue("QuantityFrom", filter.QuantityFrom);
        }

        if (filter.QuantityTo is not null)
        {
            query.Append(" AND Quantity < @QuantityTo");
            command.Parameters.AddWithValue("QuantityTo", filter.QuantityTo);
        }

        if (filter.DateFrom is not null)
        {
            query.Append(" AND Date > @DateFrom");
            command.Parameters.AddWithValue("@DateFrom", filter.DateFrom);
        }

        if (filter.DateTo is not null)
        {
            query.Append(" AND Date < @DateTo");
            command.Parameters.AddWithValue("@DateTo", filter.DateTo);
        }
    }

    private static void ApplyEntrySorting(StringBuilder query, EntrySorting sorting)
    {
        query.Append(CultureInfo.InvariantCulture, $" ORDER BY {sorting.SortColumn} {sorting.SortOrder}");
    }

    private static void ApplyPaging(StringBuilder query, SqliteCommand command, Paging paging)
    {
        query.Append(" LIMIT @PageSize OFFSET @Offset");
        command.Parameters.AddWithValue("@PageSize", paging.PageSize);
        command.Parameters.AddWithValue("@Offset", (paging.Page - 1) * paging.PageSize);
    }
}
