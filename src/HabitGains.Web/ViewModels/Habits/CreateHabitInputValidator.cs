using FluentValidation;

namespace HabitGains.Web.ViewModels.Habits;

public class CreateHabitInputValidator : AbstractValidator<CreateHabitInput>
{
    public CreateHabitInputValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100).MinimumLength(3);
        RuleFor(x => x.Measurement).NotEmpty().MaximumLength(50);
    }
}
