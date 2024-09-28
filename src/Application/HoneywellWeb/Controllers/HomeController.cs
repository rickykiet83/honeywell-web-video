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
    public async Task<JsonResult> GetAllVideos() => Json(await _videoService.GetVideoFilesAsync());

    public IActionResult Index() => View();
}