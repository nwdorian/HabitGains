using System.Reflection;
using HabitGains.Domain.Core.Primitives;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HabitGains.Web.Core.Extensions;

public static class ModelStateExtensions
{
    public static void AddErrors<T>(this ModelStateDictionary modelState, Error error, string property = "")
    {
        if (error is ValidationError validation)
        {
            foreach (Error validationError in validation.Errors)
            {
                string key = string.IsNullOrWhiteSpace(property)
                    ? validationError.Code
                    : $"{property}.{validationError.Code}";

                if (modelState.ContainsKey(key))
                {
                    modelState.AddModelError(key, validationError.Description);
                }
                else
                {
                    modelState.AddModelError(string.Empty, validationError.Description);
                }
            }
        }
        else
        {
            string[] properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToArray();

            string? name = properties.FirstOrDefault(name => error.Code.Contains(name));

            if (name is not null)
            {
                string key = string.IsNullOrWhiteSpace(property) ? error.Code : $"{property}.{name}";

                if (modelState.ContainsKey(key))
                {
                    modelState.AddModelError(key, error.Description);
                }
            }
            else
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
