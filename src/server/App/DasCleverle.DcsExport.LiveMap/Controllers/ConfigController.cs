using DasCleverle.DcsExport.LiveMap.Definitions;
using DasCleverle.Mapbox.Layers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DasCleverle.DcsExport.LiveMap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigController : Controller
{
    [HttpGet("mapbox")]
    public MapboxOptions GetMapboxConfig([FromServices] IOptionsSnapshot<MapboxOptions> options) => options.Value;

    [HttpGet("layers")]
    public IEnumerable<ILayer> GetLayers() => LayerDefinition.Layers;
}