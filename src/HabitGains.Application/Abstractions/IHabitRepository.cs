using HabitGains.Domain.Habits;

namespace HabitGains.Application.Abstractions;

public interface IHabitRepository
{
    Task BulkInsert(List<Habit> habits);
    Task<bool> Any();
}
