using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HoneywellWeb.Controllers;

public class ErrorController : Controller
{
    // GET
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