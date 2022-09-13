using DasCleverle.DcsExport.LiveMap.Localization;
using Microsoft.AspNetCore.Mvc;

namespace DasCleverle.DcsExport.LiveMap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocalesController : Controller
{
    [HttpGet]
    public async Task<IEnumerable<Locale>> GetLocales([FromServices] ILocalizationProvider localizationProvider)
    {
        return await localizationProvider.GetLocalesAsync();
    }

    [HttpGet("{locale}")]
    public async Task<ResourceCollection> GetResources(string locale, [FromServices] ILocalizationProvider localizationProvider)
    {
        return await localizationProvider.GetResourcesAsync(locale);
    }
}