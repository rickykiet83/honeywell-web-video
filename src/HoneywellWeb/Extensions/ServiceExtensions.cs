using Honeywell.DataAccess.Data;
using Honeywell.DataAccess.Repositories;
using Honeywell.DataAccess.Repositories.Interfaces;

namespace HoneywellWeb.Extensions;

public static class ServiceExtensions
{
    internal static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection");
        services.AddSqlServer<ApplicationDbContext>(connString);
    }

    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVideoRepository, VideoRepository>();
        
        return services;
    }
}