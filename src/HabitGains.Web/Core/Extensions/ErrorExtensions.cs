using HabitGains.Domain.Core.Primitives;

namespace HabitGains.Web.Core.Extensions;

public static class ErrorExtensions
{
    public static string GetProperty(this Error error)
    {
        return error.Code switch
        {
            string c when c.Contains("Habit.Name") => "Input.Name",
            string c when c.Contains("Habit.Measurement") => "Input.Measurement",
            _ => string.Empty,
        };
    }
}
