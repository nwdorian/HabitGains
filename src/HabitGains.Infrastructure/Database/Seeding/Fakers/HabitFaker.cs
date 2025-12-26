using HabitGains.Domain.Habits;

namespace HabitGains.Infrastructure.Database.Seeding.Fakers;

public static class HabitFaker
{
    public static List<Habit> Generate()
    {
        return new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Walking",
                Measurement = "Kilometers",
                Favorite = true,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Drink water",
                Measurement = "Glasses",
                Favorite = true,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Reading",
                Measurement = "Hours",
                Favorite = false,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Meditation",
                Measurement = "Hours",
                Favorite = false,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Running",
                Measurement = "Kilometers",
                Favorite = false,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Stretching",
                Measurement = "Hours",
                Favorite = false,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sleep",
                Measurement = "Hours",
                Favorite = true,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Coding",
                Measurement = "Hours",
                Favorite = true,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Cooking",
                Measurement = "Hours",
                Favorite = true,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Cleaning",
                Measurement = "Hours",
                Favorite = true,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Journaling",
                Measurement = "Hours",
                Favorite = false,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Yoga",
                Measurement = "Hours",
                Favorite = false,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Shopping",
                Measurement = "Hours",
                Favorite = true,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Praying",
                Measurement = "Hours",
                Favorite = false,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Swimming",
                Measurement = "Hours",
                Favorite = false,
            },
        };
    }
}
