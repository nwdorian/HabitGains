using HabitGains.Infrastructure;

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

        return services;
    }
}
