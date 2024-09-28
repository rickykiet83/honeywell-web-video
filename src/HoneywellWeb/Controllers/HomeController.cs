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

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IVideoRepository videoRepository)
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
            if (Path.GetExtension(file.FileName).ToLower() != ".mp4")
            {
                ModelState.AddModelError("File", "The file type is not allowed, .mp4 files only.");
                return View();
            }

            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            // Define the path to save the file
            var uploadPath = Path.Combine(wwwRootPath, "media", fileName);
            
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            await using var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create);
            await file.CopyToAsync(fileStream);
            
            var video = new VideoFile
            {
                FileName = fileName,
                FilePath = uploadPath,
                UploadedOn = DateTime.Now,
                FileType = file.ContentType
            };
            
            await _videoRepository.SaveVideoFile(video);
        }
            
        return RedirectToAction(nameof(Index));
    }
}