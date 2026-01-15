using HabitGains.Application.Habits;
using HabitGains.Application.Habits.GetById;
using HabitGains.Domain.Core.Primitives;
using HabitGains.Web.Core.Extensions;
using HabitGains.Web.ViewModels.Entries;
using HabitGains.Web.ViewModels.Habits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitGains.Web.Pages.Entries;

public class CreateModel(GetHabitByIdHandler getHabitById) : PageModel
{
    [BindProperty]
    public CreateEntryForm Input { get; set; } = default!;
    public HabitItem Habit { get; set; } = default!;
    public List<string> GetHabitErrors { get; set; } = [];
    public List<string> EntryFormErrors { get; set; } = [];

    public async Task<IActionResult> OnGet(Guid id, CancellationToken cancellationToken)
    {
        GetHabitByIdRequest getHabitByIdRequest = new(id);

        Result<HabitResponse> getHabitByIdResponse = await getHabitById.Handle(getHabitByIdRequest, cancellationToken);

        if (getHabitByIdResponse.IsFailure)
        {
            GetHabitErrors.AddRange(getHabitByIdResponse.GetErrors());
            return Page();
        }

        Habit = new HabitItem(
            getHabitByIdResponse.Value.Id,
            getHabitByIdResponse.Value.Name,
            getHabitByIdResponse.Value.Measurement,
            getHabitByIdResponse.Value.Favorite,
            getHabitByIdResponse.Value.CreatedAt
        );

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        return RedirectToPage("/Details");
    }
}
