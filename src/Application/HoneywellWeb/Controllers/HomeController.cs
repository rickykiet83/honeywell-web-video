using System.Text.Json;
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
    
    // GET: Home/GetAllVideos
    public async Task<JsonResult> GetAllVideos()
    {
        var videos = await _videoService.GetVideoFilesAsync();
        return Json(videos);
    }

    public IActionResult Index()
    {
        return View();
    }
}