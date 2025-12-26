using HabitGains.Infrastructure;
using HabitGains.Infrastructure.Database.Seeding;

namespace HabitGains.Web.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddRazorPages();

        services.AddInfrastructure(configuration);
        services.AddOptions();

        return services;
    }

    private static void AddOptions(this IServiceCollection services)
    {
        services
            .AddOptions<SeedingSettings>()
            .BindConfiguration(SeedingSettings.ConfigurationSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
