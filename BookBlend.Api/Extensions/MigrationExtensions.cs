using BookBlend.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AudiobookDbContext>();

        dbContext.Database.Migrate();
    }
}
