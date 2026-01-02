using HabitGains.Domain.Entries;

namespace HabitGains.Application.Core.Abstractions.Repositories;

public interface IEntryRepository
{
    Task BulkInsert(List<Entry> entries);
    Task<bool> Any();
}
