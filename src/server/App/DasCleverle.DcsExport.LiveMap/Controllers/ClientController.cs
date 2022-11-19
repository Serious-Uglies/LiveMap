using DasCleverle.DcsExport.Client.Abstractions.Layers;
using DasCleverle.DcsExport.Client.Abstractions.Popups;
using DasCleverle.DcsExport.Client.Icons;
using DasCleverle.Mapbox.Layers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DasCleverle.DcsExport.LiveMap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    private static readonly IContentTypeProvider ContentTypeProvider = new FileExtensionContentTypeProvider();

    [HttpGet("layers")]
    public IEnumerable<ILayer> GetLayers([FromServices] ILayerRegistry registry) => registry.GetLayers();

    [HttpGet("popup/{layer}")]
    public ActionResult GetPopup([FromServices] IPopupRegistry registry, string layer)
    {
        var popup = registry.GetPopup(layer);

        if (popup == null)
        {
            return NotFound();
        }

        return Json(popup);
    }

    [HttpGet("popup")]
    public ActionResult GetPopups([FromServices] IPopupRegistry registry)
    {
        return Json(registry.GetPopups());
    }

    [HttpGet("icon/{iconKey}")]
    public ActionResult GetIcon(string iconKey, [FromServices] IIconGenerator iconGenerator)
    {
        var key = IconKey.Parse(iconKey);
        return File(iconGenerator.GenerateIcon(key), "image/png");
    }
}