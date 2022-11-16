using System.Collections.Immutable;
using System.Collections.ObjectModel;
using DasCleverle.DcsExport.Client.Abstractions.Popups;
using DasCleverle.DcsExport.LiveMap.Caching;

namespace DasCleverle.DcsExport.LiveMap.Client;

internal class PopupRegistry : IPopupRegistry
{
    private readonly object _cacheKey = new();
    private readonly ICache _cache;
    private readonly IEnumerable<IPopupProvider> _providers;
    private readonly IEnumerable<IPopupExtender> _extenders;

    public PopupRegistry(ICache cache, IEnumerable<IPopupProvider> providers, IEnumerable<IPopupExtender> extenders)
    {
        _cache = cache;
        _providers = providers;
        _extenders = extenders;
    }

    public IPopup? GetPopup(string layer)
    {
        return GetPopups().TryGetValue(layer, out var popup) ? popup : null;
    }

    public IDictionary<string, IPopup> GetPopups()
    {
        return _cache.GetOrCreate<ImmutableDictionary<string, IPopup>>(_cacheKey, (entry) =>
        {
            var popups = ImmutableDictionary.CreateBuilder<string, IPopup>();
            var providersByLayer = _providers.GroupBy(x => x.Layer);

            foreach (var providers in providersByLayer)
            {
                if (providers.Count() > 1)
                {
                    throw new InvalidOperationException($"Found multiple registered popup providers for layer '{providers.Key}'");
                }

                var provider = providers.First();
                var extenders = _extenders.Where(x => x.Layer == provider.Layer);

                var popup = provider.GetPopup();

                foreach (var extender in extenders)
                {
                    popup = extender.Extend(popup);
                }

                popups[provider.Layer] = popup;
            }

            return popups.ToImmutable();
        });
    }
}
