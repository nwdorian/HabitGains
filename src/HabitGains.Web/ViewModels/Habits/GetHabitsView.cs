using System.ComponentModel.DataAnnotations;

namespace HabitGains.Web.ViewModels.Habits;

public class GetHabitsView(string name, string measurement, bool favorite, DateTime createdAt)
{
    public string? Name { get; set; } = name;
    public string? Measurement { get; set; } = measurement;
    public bool Favorite { get; set; } = favorite;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy H:mm}")]
    public DateTime CreatedAt { get; } = createdAt;
}
