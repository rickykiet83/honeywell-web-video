using Honeywell.Models;
using Microsoft.EntityFrameworkCore;

namespace Honeywell.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<VideoFile> VideoFiles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VideoFile>().HasData(
            new VideoFile
            {
                Id = 1,
                FileName = "big_buck_bunny",
                FilePath = "media/big_buck_bunny/big_buck_bunny.mp4"
            }
        );
    }
}