using HabitGains.Infrastructure.Database.ConnectionFactory;
using HabitGains.Infrastructure.Database.Initializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HabitGains.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString =
            configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Invalid connection string.");

        services.AddSingleton<IDbConnectionFactory>(_ => new DbConnectionFactory(connectionString));
        services.AddScoped<IDbInitializer, DbInitializer>();
    }
}
