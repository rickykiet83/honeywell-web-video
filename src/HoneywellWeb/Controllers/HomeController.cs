using System.Diagnostics;
using Honeywell.DataAccess.Repositories.Interfaces;
using Honeywell.Models;
using Microsoft.AspNetCore.Mvc;
using HoneywellWeb.Models;

namespace HoneywellWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IVideoRepository _videoRepository;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment,
        IVideoRepository videoRepository)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        _videoRepository = videoRepository;
    }

    public async Task<IActionResult> Index()
    {
        var videos = await _videoRepository.GetVideosAsync(false);
        return View(videos);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // GET: Show form to upload a new video
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(List<IFormFile>? files)
    {
        if (files == null) return View();

        var wwwRootPath = _webHostEnvironment.WebRootPath;
        foreach (IFormFile file in files)
        {
            // Check if the uploaded file is an MP4
            if (Path.GetExtension(file.FileName).ToLower() != ".mp4")
            {
                ModelState.AddModelError("File", "The file type is not allowed, .mp4 files only.");
                return View();
            }

            // Get the file name without the extension
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

            // Define the folder path to save the file, each video will have its own folder
            var folderPath = Path.Combine(wwwRootPath, "media", fileNameWithoutExtension);

            // Ensure the directory exists (create if it doesn't)
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Combine the folder path and the full file name (file.mp4)
            var fullFilePath = Path.Combine(folderPath, file.FileName);

            // Save the file to the created directory
            await using var fileStream = new FileStream(fullFilePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            // Set the relative path to store in the database (for example: media/filename/filename.mp4)
            var relativeFilePath = Path.Combine("media", fileNameWithoutExtension, file.FileName).Replace("\\", "/");

            // Create the video entity to save to the database
            var video = new VideoFile
            {
                FileName = fileNameWithoutExtension, // Store filename without extension
                FilePath = relativeFilePath, // Path to be stored in the database
                FileType = file.ContentType // Content type (e.g., video/mp4)
            };

            // Save the video details to the database
            await _videoRepository.SaveVideoFile(video);
        }

        return RedirectToAction(nameof(Index));
    }
}