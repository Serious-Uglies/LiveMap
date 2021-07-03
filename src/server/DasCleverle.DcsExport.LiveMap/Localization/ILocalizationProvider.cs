using System.Threading.Tasks;

namespace DasCleverle.DcsExport.LiveMap.Localization
{
    public interface ILocalizationProvider
    {
        Task<ResourceCollection> GetLocalizationAsync(string locale, string ns);
    }
}