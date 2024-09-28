namespace Honeywell.Utility.Helpers;

public static class CommonUtility
{
    public static string GetReadableFileSize(decimal fileSize)
    {
        const decimal KB = 1024m;
        const decimal MB = KB * 1024m;

        return fileSize switch
        {
            < KB => $"{fileSize:F2} bytes",
            < MB => $"{fileSize / KB:F2} KB",
            _ => $"{fileSize / MB:F2} MB"
        };
    }
}