using Honeywell.DataAccess.Repositories.Interfaces;
using Honeywell.Models;
using Honeywell.Models.ViewModels;
using Honeywell.Utility.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Service.Contracts.Interfaces;

namespace Service;

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videoRepository;
    private readonly ILogger<VideoService> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IVideoValidator _videoValidator;

    public VideoService(IVideoRepository videoRepository, ILogger<VideoService> logger,
        IWebHostEnvironment webHostEnvironment, IVideoValidator videoValidator)
    {
        _videoRepository = videoRepository;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        _videoValidator = videoValidator;
    }

    public async Task<ServiceResult> UploadVideoFileAsync(List<IFormFile> files)
    {
        var result = new ServiceResult();
        var rootPath = _webHostEnvironment.WebRootPath;
        _logger.LogInformation("Uploading {Count} files", files.Count);

        if (!CheckFilesValid(files, result, out var serviceResult))
            return serviceResult;

        foreach (var file in files)
        {
            try
            {
                // Generate the directory path for the video (e.g., videos/filename)
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var fileNameWithExtension = Path.GetFileName(file.FileName);
                var folderPath = Path.Combine(rootPath, SystemConstants.DefaultVideoPath);

                // Create the directory if it doesn't exist
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // Combine folder path and file name to get the full file path
                var fullFilePath = Path.Combine(folderPath, file.FileName);

                // Save the file to the created directory
                await using var fileStream = new FileStream(fullFilePath, FileMode.Create);
                await file.CopyToAsync(fileStream);

                // Set the relative path for storing in the database (normalize path separators)
                var relativeFilePath = Path
                    .Combine(SystemConstants.DefaultVideoPath, file.FileName)
                    .Replace("\\", "/");

                await SaveVideoAsync(fileNameWithExtension, relativeFilePath, file.ContentType, file.Length);
                result.Success = true; // Indicate success
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the file {FileName}", file.FileName);
                throw new Exception($"An error occurred while processing the file {file.FileName}: {ex.Message}");
            }
        }

        return result;
    }

    // Check if the uploaded files are valid
    private bool CheckFilesValid(List<IFormFile> files, ServiceResult result, out ServiceResult serviceResult)
    {
        serviceResult = result;
        foreach (var file in files)
        {
            var validationResult = _videoValidator.ValidateVideo(file);
            if (validationResult.Success) continue;

            serviceResult = validationResult;
            return false;
        }

        return true;
    }

    private async Task SaveVideoAsync(string fileNameWithExtension, string relativeFilePath, string contentType,
        long fileSizeInMb)
    {
        var fileNameExisted = _videoRepository
            .FindByCondition(x => x.FileName.Equals(fileNameWithExtension), false)
            .Any();
        
        if (fileNameExisted)
            return;

        // Create the video entity
        var video = new VideoFile
        {
            FileName = fileNameWithExtension, // Store filename with extension
            FilePath = relativeFilePath, // Relative path for database storage
            FileType = contentType, // Content type (e.g., video/mp4)
            FileSizeInMb = fileSizeInMb, // File size in MB
        };

        // Save the video entity to the database
        await _videoRepository.SaveVideoFile(video);
    }

    public async Task<IEnumerable<VideoVM>> GetVideoFilesAsync()
    {
        var videoFiles = await _videoRepository.GetVideosAsync(false);
        return videoFiles.Select(x => new VideoVM(x.Id, x.FileName, x.FilePath, x.FileSizeInMb));
    }
}