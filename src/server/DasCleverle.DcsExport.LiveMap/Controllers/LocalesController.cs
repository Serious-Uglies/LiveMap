using System.Threading.Tasks;
using DasCleverle.DcsExport.LiveMap.Localization;
using Microsoft.AspNetCore.Mvc;

namespace DasCleverle.DcsExport.LiveMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalesController : Controller
    {
        [HttpGet("{locale}/{ns}")]
        public async Task<ResourceCollection> GetLocalization(
            string locale,
            string ns,
            [FromServices] ILocalizationProvider localizationProvider
        )
        {
            return await localizationProvider.GetLocalizationAsync(locale, ns);
        }
    }
}