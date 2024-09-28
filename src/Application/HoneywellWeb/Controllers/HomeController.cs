using System.Diagnostics;
using Honeywell.DataAccess.Repositories.Interfaces;
using Honeywell.Models;
using Honeywell.Utility.Settings;
using Microsoft.AspNetCore.Mvc;
using HoneywellWeb.Models;
using Service.Contracts;

namespace HoneywellWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IVideoService _videoService;

    public HomeController(ILogger<HomeController> logger, 
        IVideoService videoService)
    {
        _logger = logger;
        _videoService = videoService;
    }

    public async Task<IActionResult> Index()
    {
        var videos = await _videoService.GetVideoFilesAsync();
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

        foreach (IFormFile file in files)
        {
            // Check if the uploaded file is an MP4
            if (Path.GetExtension(file.FileName).ToLower() != ".mp4")
            {
                ModelState.AddModelError("File", "The file type is not allowed, .mp4 files only.");
                return View();
            }
            
            // Check if the file size is greater than 200MB
            if (file.Length > 200 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "The file size is too large, 200MB maximum.");
                return View();
            }

            await _videoService.SaveVideoFileAsync(files);
        }

        return RedirectToAction(nameof(Index));
    }
}