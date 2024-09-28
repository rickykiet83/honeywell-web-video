using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HoneywellWeb.Models;
using Service.Contracts.Interfaces;

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
        if (files == null || files.Count == 0)
        {
            ModelState.AddModelError("File", "Please upload at least one file.");
            return View();
        }

        var result = await _videoService.SaveVideoFileAsync(files);
        
        // If there are validation errors, add them to the ModelState and return the view
        if (result.Success) return RedirectToAction(nameof(Index));
        
        foreach (var error in result.Errors)
            ModelState.AddModelError("File", error);

        return View(); // Return the view to show errors
    }
}