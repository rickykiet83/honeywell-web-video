using Honeywell.Models;
using Honeywell.Utility.Settings;
using Microsoft.EntityFrameworkCore;

namespace Honeywell.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<VideoFile> VideoFiles => Set<VideoFile>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VideoFile>()
            .Property(v => v.Id)
            .ValueGeneratedOnAdd();
        
        // Set precision for the FileSizeInMB property (precision: 18 digits, 2 after the decimal)
        modelBuilder.Entity<VideoFile>()
            .Property(v => v.FileSizeInMb)
            .HasPrecision(18, 2);
        
        // Set the FileName property to be unique
        modelBuilder.Entity<VideoFile>()
            .HasIndex(v => v.FileName)
            .IsUnique();
        
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VideoFile>().HasData(
            new VideoFile
            {
                Id = 1,
                FileName = "big_buck_bunny.mp4",
                FilePath = $"{SystemConstants.DefaultVideoPath}/big_buck_bunny.mp4",
                FileSizeInMb = 5510872 // 5.25 MB
            }
        );
    }
}