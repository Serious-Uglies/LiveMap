using DasCleverle.DcsExport.LiveMap.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DasCleverle.DcsExport.LiveMap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebugController : Controller
{
    private readonly ICache _cache;
    private readonly IWebHostEnvironment _environment;

    public DebugController(ICache cache, IWebHostEnvironment environment)
    {
        _cache = cache;
        _environment = environment;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (_environment.IsDevelopment())
        {
            return;
        }

        context.Result = new StatusCodeResult(404);
    }

    [HttpGet("cache/clear")]
    public void ClearCaches() => _cache.Clear();
}
