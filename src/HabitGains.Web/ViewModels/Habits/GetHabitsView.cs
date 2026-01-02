using HabitGains.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HabitGains.Web.ViewModels.Habits;

public class GetHabitsView
{
    public required IReadOnlyList<HabitItem> Habits { get; init; }
    public required Metadata Metadata { get; init; }

    public string? SortColumn { get; init; }
    public string? SortOrder { get; init; }
    public string? SearchTerm { get; init; }

    public required SelectList PageSizeOptions { get; init; }

    public required SelectList MeasurementOptions { get; init; }
    public string? MeasurementFilter { get; init; }

    public required SelectList FavoriteOptions { get; init; }
    public string? FavoriteFilter { get; init; }
}
