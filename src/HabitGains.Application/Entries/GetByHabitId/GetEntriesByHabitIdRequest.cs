namespace HabitGains.Application.Entries.GetByHabitId;

public record GetEntriesByHabitIdRequest(
    Guid HabitId,
    int? QuantityFrom,
    int? QuantityTo,
    DateTime? DateFrom,
    DateTime? DateTo,
    int Page,
    int PageSize,
    string SortColumn,
    string SortOrder
);
