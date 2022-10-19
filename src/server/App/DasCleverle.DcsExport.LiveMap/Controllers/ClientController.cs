using System.Net;
using DasCleverle.DcsExport.Client.Abstractions.Popups;
using DasCleverle.DcsExport.LiveMap.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DasCleverle.DcsExport.LiveMap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    private static readonly IContentTypeProvider ContentTypeProvider = new FileExtensionContentTypeProvider();

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

    [HttpGet("icons")]
    public IEnumerable<IconInfo> GetIcons([FromServices] IIconProvider iconProvider)
    {
        return iconProvider.GetIcons();
    }

    [HttpGet("icons/{fileName}")]
    public ActionResult GetIcons([FromServices] IIconProvider iconProvider, string fileName)
    {
        var file = iconProvider.GetIconFile(fileName);

        if (file == null || !ContentTypeProvider.TryGetContentType(fileName, out var contentType))
        {
            return StatusCode((int)HttpStatusCode.NotFound);
        }

        return File(file, contentType);
    }
}