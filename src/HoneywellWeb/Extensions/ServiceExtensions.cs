using Honeywell.DataAccess.Data;

namespace HoneywellWeb.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection");
        services.AddSqlServer<ApplicationDbContext>(connString);
    }
}