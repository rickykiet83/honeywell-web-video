using Microsoft.AspNetCore.Mvc;
using Service.Contracts.Interfaces;

namespace HoneywellWeb.Controllers;

public class HomeController : Controller
{
    private readonly IVideoService _videoService;

    public HomeController(IVideoService videoService)
    {
        _videoService = videoService;
    }

    public async Task<IActionResult> Index()
    {
        var videos = await _videoService.GetVideoFilesAsync();
        return View(videos);
    }
}