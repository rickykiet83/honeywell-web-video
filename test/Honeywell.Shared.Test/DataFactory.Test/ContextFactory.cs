using Honeywell.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataFactory.Test;

public class ContextFactory
{
    public static ApplicationDbContext Create()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new ApplicationDbContext(options);
        return context;
    }
}