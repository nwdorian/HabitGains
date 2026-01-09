namespace HabitGains.Application.Core.Pagination.Entries;

public record EntryFilter(int? QuantityFrom, int? QuantityTo, DateOnly? DateFrom, DateOnly? DateTo);
