using FluentValidation;

namespace HabitGains.Web.ViewModels.Habits;

public class GetHabitsQueryValidator : AbstractValidator<GetHabitsQuery>
{
    private static readonly string[] _allowedSortColumns = { "name", "measurement", "favorite", "created" };

    public GetHabitsQueryValidator()
    {
        RuleFor(x => x.CurrentPage)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than zero.")
            .LessThan(1000);
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(3)
            .WithMessage("Page size must be greater than or equal to 3.")
            .LessThanOrEqualTo(20)
            .WithMessage("Page size must be less than or equal to 20.");
        RuleFor(x => x.SortColumn).Must(c => _allowedSortColumns.Contains(c)).WithMessage("Invalid sort column.");
        RuleFor(x => x.SortOrder).Must(o => o is "asc" or "desc").WithMessage("Sort order must be 'asc' or 'desc'.");
        RuleFor(x => x.SearchTerm).MaximumLength(100).WithMessage("Search term can have a maximum of 100 characters.");
    }
}
