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
    public IActionResult Index()
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
        
        // If upload is successful, redirect to Home/Index
        if (result.Success) 
            return RedirectToAction("Index", "Home");
        
        foreach (var error in result.Errors)
            ModelState.AddModelError("File", error);

        return View(); // Return the view to show errors
    }
}