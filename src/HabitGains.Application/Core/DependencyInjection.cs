using HabitGains.Application.Habits.Get;
using Microsoft.Extensions.DependencyInjection;

namespace HabitGains.Application.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHabitUseCases();

        return services;
    }

    private static void AddHabitUseCases(this IServiceCollection services)
    {
        services.AddScoped<GetHabits>();
    }
}
