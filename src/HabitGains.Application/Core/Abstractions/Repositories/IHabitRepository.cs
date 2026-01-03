using HabitGains.Application.Core.Pagination;
using HabitGains.Application.Core.Pagination.Habits;
using HabitGains.Domain.Habits;

namespace HabitGains.Application.Core.Abstractions.Repositories;

public interface IHabitRepository
{
    Task<IReadOnlyList<Habit>> GetHabitsPage(
        HabitFilter filter,
        HabitSorting sorting,
        Paging paging,
        CancellationToken cancellationToken
    );
    Task<int> Count(HabitFilter filter, CancellationToken cancellationToken);
    Task<List<string>> GetHabitMeasurements(CancellationToken cancellationToken);
    Task BulkInsert(List<Habit> habits);
    Task<bool> Any();
}
