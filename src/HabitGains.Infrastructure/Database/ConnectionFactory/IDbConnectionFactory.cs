using System.Data;

namespace HabitGains.Infrastructure.Database.ConnectionFactory;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}
