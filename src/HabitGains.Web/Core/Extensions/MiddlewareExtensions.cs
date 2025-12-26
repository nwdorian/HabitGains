using HabitGains.Infrastructure.Database.Initializer;
using HabitGains.Infrastructure.Database.Seeding;

namespace HabitGains.Web.Core.Extensions;

public static class MiddlewareExtensions
{
    public static async Task<IApplicationBuilder> UseWebApplicationMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages().WithStaticAssets();

        await app.InitializeDatabaseAsync();
        await app.SeedDatabaseAsync();

        return app;
    }

    private static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        IDbInitializer initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

        await initializer.RunAsync();
    }

    private static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        IDbSeeder seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();

        await seeder.RunAsync();
    }
}
