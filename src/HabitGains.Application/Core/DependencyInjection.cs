using FluentValidation;
using HabitGains.Application.Habits.Create;
using HabitGains.Application.Habits.Get;
using Microsoft.Extensions.DependencyInjection;

namespace HabitGains.Application.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHabitUseCases();
        services.AddFluentValidation();

        return services;
    }

    private static void AddHabitUseCases(this IServiceCollection services)
    {
        services.AddScoped<GetHabitsHandler>();
        services.AddScoped<CreateHabitHandler>();
    }

    private static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
}
