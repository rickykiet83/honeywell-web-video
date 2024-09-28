using System.Diagnostics;

namespace HoneywellWeb.Middlewares;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = new Stopwatch();

        try
        {
            stopWatch.Start();
            await _next(context);
        }
        finally
        {
            stopWatch.Stop();

            var seconds = stopWatch.Elapsed.Seconds;
            if (seconds > 3) // if the request is greater than 3 seconds, then log the warnings
                _logger.LogWarning(
                    "{RequestMethod} {RequestPath} request took {Seconds}s to complete",
                    context.Request.Method,
                    context.Request.Path,
                    seconds);
        }
    }
}