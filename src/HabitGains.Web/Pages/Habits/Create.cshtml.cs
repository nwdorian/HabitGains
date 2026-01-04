using FluentValidation;
using FluentValidation.Results;
using HabitGains.Application.Habits.Create;
using HabitGains.Domain.Core.Primitives;
using HabitGains.Web.Core.Extensions;
using HabitGains.Web.ViewModels.Habits;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HabitGains.Web.Pages.Habits;

public class CreateModel(CreateHabit useCase, IValidator<CreateHabitInput> validator) : PageModel
{
    [BindProperty]
    public CreateHabitInput Input { get; set; } = default!;

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        ValidationResult result = await validator.ValidateAsync(Input, cancellationToken);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return Page();
        }

        CreateHabitRequest request = new(Input.Name, Input.Measurement, Input.Favorite);

        Result response = await useCase.Handle(request, cancellationToken);

        if (response.IsFailure)
        {
            ModelState.AddModelError(response.Error.GetProperty(), response.Error.Description);
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
