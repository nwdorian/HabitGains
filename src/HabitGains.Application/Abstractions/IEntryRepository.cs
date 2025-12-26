using HabitGains.Domain.Entries;

namespace HabitGains.Application.Abstractions;

public interface IEntryRepository
{
    Task BulkInsert(List<Entry> entries);
    Task<bool> Any();
}
