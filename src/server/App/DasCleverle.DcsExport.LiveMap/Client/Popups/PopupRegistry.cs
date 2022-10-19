using System.Collections.ObjectModel;
using DasCleverle.DcsExport.Client.Abstractions.Popups;

namespace DasCleverle.DcsExport.LiveMap.Client.Popups;

internal class PopupRegistry : IPopupRegistry
{
    private readonly Dictionary<string, IPopup> _cache = new();
    private bool _isInitialized = false;
    private object _syncRoot = new();

    private readonly IEnumerable<IPopupProvider> _providers;
    private readonly IEnumerable<IPopupExtender> _extenders;

    public PopupRegistry(IEnumerable<IPopupProvider> providers, IEnumerable<IPopupExtender> extenders)
    {
        _providers = providers;
        _extenders = extenders;
    }

    public IPopup? GetPopup(string layer)
    {
        Initialize();
        return _cache.TryGetValue(layer, out var popup) ? popup : null;
    }

    public IDictionary<string, IPopup> GetPopups()
    {
        Initialize();
        return new ReadOnlyDictionary<string, IPopup>(_cache);
    }

    private void Initialize() 
    {
        if (_isInitialized)
        {
            return;
        }

        lock (_syncRoot)
        {
            if (_isInitialized)
            {
                return;
            }

            var providersByLayer = _providers.GroupBy(x => x.Layer);

            foreach (var providers in providersByLayer)
            {
                if (providers.Count() > 1)
                {
                    throw new InvalidOperationException($"Found multiple registered popup providers for layer '{providers.Key}'");
                }

                var provider = providers.First();
                var extenders = _extenders.Where(x => x.Layer == provider.Layer);

                var builder = provider.GetPopup();

                foreach (var extender in extenders)
                {
                    extender.Extend(builder);
                }

                _cache[provider.Layer] = builder.Build();
            }

            _isInitialized = true;
        }
    }
}
