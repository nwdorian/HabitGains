namespace HabitGains.Web.ViewModels.Shared;

public record Metadata(int Page, int PageSize, int TotalCount, int TotalPages, bool HasPreviousPage, bool HasNextPage);
