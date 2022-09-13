using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DasCleverle.DcsExport.LiveMap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigController : Controller
{
    [HttpGet("mapbox")]
    public MapboxOptions GetMapboxConfig([FromServices] IOptionsSnapshot<MapboxOptions> options)
    {
        return options.Value;
    }
}