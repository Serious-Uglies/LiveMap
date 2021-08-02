using System.Collections.Generic;
using System.Threading.Tasks;

namespace DasCleverle.DcsExport.LiveMap.Localization
{
    public interface ILocalizationProvider
    {
        Task<IEnumerable<Locale>> GetLocalesAsync();

        Task<ResourceCollection> GetResourcesAsync(string locale);
    }
}