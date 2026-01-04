using System.ComponentModel.DataAnnotations;

namespace HabitGains.Web.ViewModels.Habits;

public class CreateHabitInput
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Measurement { get; set; } = string.Empty;
    public bool Favorite { get; set; }
}
