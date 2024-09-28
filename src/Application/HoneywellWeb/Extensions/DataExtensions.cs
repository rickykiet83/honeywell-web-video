using Honeywell.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace HoneywellWeb.Extensions;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync().ConfigureAwait(false);

        var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("DB Migration has been completed");
        logger.LogInformation(LogLevel.Critical.GetHashCode(), "The database is ready!");
    }
}