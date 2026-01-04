using HabitGains.Domain.Core.Primitives;

namespace HabitGains.Domain.Habits;

public static class HabitErrors
{
    public static Error NameNotUnique(string name) =>
        Error.Conflict("Habit.Name.AlreadyExists", $"The habit with a name {name} already exists!");
}
