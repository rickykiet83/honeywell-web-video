using Bogus;
using Honeywell.Models;
using Honeywell.Utility.Settings;

namespace DataFactory.Test;

public static class VideoFactory
{
    public static readonly Faker<VideoFile> GetVideos = new Faker<VideoFile>()
        .RuleFor(v => v.Id, f => f.Random.Int(1, 1000))
        // First, define the file type
        .RuleFor(v => v.FileType, f => f.PickRandom(new[] { "video/mp4", "video/wmv", "video/avi", "video/mkv" }))
        // Then, generate a file name based on the chosen file type's extension
        .RuleFor(v => v.FileName, (f, v) => $"video_{f.UniqueIndex}{GetFileExtension(v.FileType)}")
        // Generate a valid file path (modify to match your actual path structure)
        .RuleFor(v => v.FilePath, (f, v) => $"/{SystemConstants.DefaultVideoPath}/{v.FileName}")
        // Generate a random file size in MB
        .RuleFor(v => v.FileSizeInMb, f => f.Random.Decimal(1, SystemConstants.MaxVideoSize))

        // Generate a random past upload date
        .RuleFor(v => v.UploadedOn, f => f.Date.Past());

    public static readonly Faker<VideoFile> GetVideosWithMp4 = GetVideos.Clone()
        // Generate a unique file name for MP4 files
        .RuleFor(v => v.FileName, f => $"video_{f.UniqueIndex}.mp4")
        // Override the FileType rule to always use "video/mp4"
        .RuleFor(v => v.FileType, f => "video/mp4");

    // Helper method to get the file extension based on the file type
    private static string GetFileExtension(string fileType)
    {
        return fileType switch
        {
            "video/mp4" => ".mp4",
            "video/wmv" => ".wmv",
            "video/avi" => ".avi",
            "video/mkv" => ".mkv",
            _ => ".mp4" // Default case if no match
        };
    }
}