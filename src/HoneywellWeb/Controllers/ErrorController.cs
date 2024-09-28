using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HoneywellWeb.Controllers;

[ApiExplorerSettings(IgnoreApi = true)] // Exclude from Swagger
public class ErrorController : Controller
{
    // GET
    // This method will not be directly accessible via a URL
    [NonAction]
    public IActionResult Index()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionFeature != null)
        {
            // Get details about the exception that occurred
            ViewBag.ErrorMessage = exceptionFeature.Error.Message;
            ViewBag.RouteOfException = exceptionFeature.Path;
        }
            
        return View();
    }
}