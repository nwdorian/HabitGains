using FluentValidation;
using FluentValidation.Results;
using HabitGains.Application.Core.Abstractions.Repositories;
using HabitGains.Application.Habits.Get;
using HabitGains.Web.Core.Constants;
using HabitGains.Web.Core.Extensions;
using HabitGains.Web.ViewModels.Habits;
using HabitGains.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HabitGains.Web.Pages.Habits;

public class IndexModel(GetHabits useCase, IHabitRepository habitRepository, IValidator<GetHabitsQuery> validator)
    : PageModel
{
    public required IReadOnlyList<HabitItem> Habits { get; set; }
    public Metadata Metadata { get; set; } = default!;
    public SelectList PageSizeOptions { get; set; } = default!;
    public SelectList MeasurementOptions { get; set; } = default!;
    public SelectList FavoriteOptions { get; set; } = default!;
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
    public string? SearchTerm { get; set; }
    public string? MeasurementFilter { get; set; }
    public string? FavoriteFilter { get; set; }

    public async Task<IActionResult> OnGetAsync(GetHabitsQuery query, CancellationToken cancellationToken = default)
    {
        ValidationResult result = await validator.ValidateAsync(query, cancellationToken);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return Page();
        }

        bool? favorite = query.Favorite switch
        {
            "true" => true,
            "false" => false,
            _ => null,
        };

        GetHabitsRequest request = new(
            query.Measurement,
            favorite,
            query.SearchTerm,
            query.CurrentPage,
            query.PageSize,
            query.SortColumn,
            query.SortOrder
        );

        GetHabitsResponse response = await useCase.Handle(request, cancellationToken);

        Habits = response
            .Items.Select(h => new HabitItem(h.Id, h.Name, h.Measurement, h.Favorite, h.CreatedAt))
            .ToList();

        Metadata = new Metadata(
            response.Page,
            response.PageSize,
            response.TotalCount,
            response.TotalPages,
            response.HasPreviousPage,
            response.HasNextPage
        );

        SortColumn = query.SortColumn;
        SortOrder = query.SortOrder;
        SearchTerm = query.SearchTerm;

        MeasurementFilter = query.Measurement;
        MeasurementOptions = new SelectList(
            await habitRepository.GetHabitMeasurements(cancellationToken),
            selectedValue: query.Measurement
        );

        FavoriteFilter = query.Favorite;
        FavoriteOptions = new SelectList(
            new[]
            {
                new { Value = "", Text = "All" },
                new { Value = "true", Text = "Favorite" },
                new { Value = "false", Text = "Not Favorite" },
            },
            "Value",
            "Text",
            query.Favorite
        );

        PageSizeOptions = new SelectList(PagingDefaults.PageSizeOptions, selectedValue: query.PageSize);

        return Page();
    }
}
