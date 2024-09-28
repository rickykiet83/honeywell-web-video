using Honeywell.Utility.Helpers;

namespace Honeywell.Models.ViewModels;

public record VideoVM(int Id, string FileName, string FilePath, decimal FileSizeInMb)
{
    public string FileSize => CommonUtility.GetReadableFileSize(FileSizeInMb);
    public string FileNameWithoutExtension => Path.GetFileNameWithoutExtension(FileName);
}
