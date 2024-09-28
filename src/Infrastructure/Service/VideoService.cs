using Honeywell.DataAccess.Repositories.Interfaces;
using Honeywell.Models;
using Honeywell.Models.ViewModels;
using Honeywell.Utility.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Service.Contracts;

namespace Service;

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videoRepository;
    private readonly ILogger<VideoService> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public VideoService(IVideoRepository videoRepository, ILogger<VideoService> logger, IWebHostEnvironment webHostEnvironment)
    {
        _videoRepository = videoRepository;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }


    public async Task SaveVideoFileAsync(List<IFormFile> files)
    {
        var wwwRootPath = _webHostEnvironment.WebRootPath;
        foreach (IFormFile file in files)
        {
            // Check if the uploaded file is an MP4
            // if (Path.GetExtension(file.FileName).ToLower() != ".mp4")
            // {
            //     ModelState.AddModelError("File", "The file type is not allowed, .mp4 files only.");
            //     return View();
            // }
            //
            // // Check if the file size is greater than 200MB
            // if (file.Length > 200 * 1024 * 1024)
            // {
            //     ModelState.AddModelError("File", "The file size is too large, 200MB maximum.");
            //     return View();
            // }

            // Get the file name without the extension
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

            // Define the folder path to save the file, each video will have its own folder
            var folderPath = Path.Combine(wwwRootPath, SystemConstants.DefaultVideoPath, fileNameWithoutExtension);

            // Ensure the directory exists (create if it doesn't)
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Combine the folder path and the full file name (file.mp4)
            var fullFilePath = Path.Combine(folderPath, file.FileName);

            // Save the file to the created directory
            await using var fileStream = new FileStream(fullFilePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            // Set the relative path to store in the database (for example: media/filename/filename.mp4)
            var relativeFilePath = Path.Combine(SystemConstants.DefaultVideoPath, fileNameWithoutExtension, file.FileName).Replace("\\", "/");

            // Create the video entity to save to the database
            var video = new VideoFile
            {
                FileName = fileNameWithoutExtension, // Store filename without extension
                FilePath = relativeFilePath, // Path to be stored in the database
                FileType = file.ContentType, // Content type (e.g., video/mp4)
                FileSizeInMb = file.Length, // File size in bytes
            };

            // Save the video details to the database
            await _videoRepository.SaveVideoFile(video);
        }
    }

    public async Task<IEnumerable<VideoVM>> GetVideoFilesAsync()
    {
        var videoFiles = await _videoRepository.GetVideosAsync(false);
        return videoFiles.Select(x => new VideoVM(x.Id, x.FileName, x.FilePath, x.FileSizeInMb));
    }
}