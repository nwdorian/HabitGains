using HabitGains.Application.Abstractions;
using HabitGains.Domain.Entries;
using HabitGains.Domain.Habits;
using HabitGains.Infrastructure.Database.Seeding.Fakers;
using Microsoft.Extensions.Options;

namespace HabitGains.Infrastructure.Database.Seeding;

public class DbSeeder(
    IHabitRepository habitRepository,
    IEntryRepository entryRepository,
    IOptions<SeedingSettings> settings
) : IDbSeeder
{
    public async Task RunAsync()
    {
        if (!settings.Value.SeedOnStartup)
        {
            return;
        }

        if (await habitRepository.Any() || await entryRepository.Any())
        {
            return;
        }

        List<Habit> habits = HabitFaker.Generate();
        List<Entry> entries = new();

        foreach (Habit habit in habits)
        {
            entries.AddRange(
                new HabitEntryFaker()
                    .FinishWith((_, y) => y.HabitId = habit.Id)
                    .Generate(settings.Value.EntriesPerHabit)
            );
        }

        await habitRepository.BulkInsert(habits);
        await entryRepository.BulkInsert(entries);
    }
}
