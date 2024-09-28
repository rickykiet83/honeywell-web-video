using System.ComponentModel.DataAnnotations;

namespace Honeywell.Models;

public class VideoFile
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string FileName { get; set; } = null!; // Name of the video file

    [Required]
    [StringLength(500)]
    public string FilePath { get; set; } = null!; // Path to the file in the media folder

    [DataType(DataType.DateTime)]
    public DateTime UploadedOn { get; set; } = DateTime.UtcNow; // Date when the video was uploaded

    [StringLength(255)]
    public string FileType { get; set; } = "video/mp4"; // File type (always mp4 for now)
    
    public decimal FileSizeInMb { get; set; } // File size in megabytes
}