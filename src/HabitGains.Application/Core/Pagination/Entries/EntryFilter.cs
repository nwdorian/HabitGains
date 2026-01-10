namespace HabitGains.Application.Core.Pagination.Entries;

public record EntryFilter(int? QuantityFrom, int? QuantityTo, DateTime? DateFrom, DateTime? DateTo);
