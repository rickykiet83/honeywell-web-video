using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Interfaces;

namespace HoneywellWeb.Controllers;

public class UploadController : Controller
{
    private readonly IVideoService _videoService;

    public UploadController(IVideoService videoService)
    {
        _videoService = videoService;
    }
    
    // GET
    public IActionResult Index() => View();
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(List<IFormFile>? files)
    {
        if (files == null || files.Count == 0)
        {
            ModelState.AddModelError("File", "Please upload at least one file.");
            return View(nameof(Index));
        }

        await Task.Delay(3000); // Simulate a delay of 3 seconds (for loading spinner)
        var result = await _videoService.UploadVideoFileAsync(files);
        
        // If upload is successful, redirect to Home/Index
        if (result.Success) 
            return RedirectToAction("Index", "Home");
        
        foreach (var error in result.Errors)
            ModelState.AddModelError("File", error);

        return View(nameof(Index)); // Return the view to show errors
    }
}