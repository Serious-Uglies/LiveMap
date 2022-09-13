namespace DasCleverle.DcsExport.LiveMap.Localization;

public interface ILocalizationProvider
{
    Task<IEnumerable<Locale>> GetLocalesAsync();

    Task<ResourceCollection> GetResourcesAsync(string locale);
}