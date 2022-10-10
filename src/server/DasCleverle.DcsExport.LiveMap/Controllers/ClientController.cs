using DasCleverle.DcsExport.Client.Abstractions.Popups;
using Microsoft.AspNetCore.Mvc;

namespace DasCleverle.DcsExport.LiveMap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
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
}
