using Honeywell.Utility.Settings;
using Microsoft.AspNetCore.Http;
using Service.Contracts.Interfaces;

namespace Service.Contracts;

public class VideoValidatorService : IVideoValidator
{
    public ServiceResult ValidateVideo(IFormFile file)
    {
        var result = new ServiceResult();
        CheckExtensions(file, result);
        CheckFileSize(file, result);

        return result;
    }

    private static void CheckFileSize(IFormFile file, ServiceResult result)
    {
        // Check if the file size is greater than MaxVideoSize (200MB)
        if (file.Length > SystemConstants.MaxVideoSize)
            result.AddError($"The file size of file name: {file.FileName} is too large, {SystemConstants.MaxVideoSize}MB maximum.");
    }

    private static void CheckExtensions(IFormFile file, ServiceResult result)
    {
        // Check if the uploaded file is an MP4
        if (Path.GetExtension(file.FileName).ToLower() != SystemConstants.DefaultVideoExtension)
            result.AddError($"The file type of file name: {file.FileName} is not allowed, {SystemConstants.DefaultVideoExtension} files only.");
    }
}