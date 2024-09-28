using Honeywell.Models;
using Microsoft.EntityFrameworkCore;

namespace Honeywell.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<VideoFile> VideoFiles { get; set; }
}