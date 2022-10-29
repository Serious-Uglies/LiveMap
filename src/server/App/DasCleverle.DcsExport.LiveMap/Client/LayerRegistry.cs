using DasCleverle.DcsExport.Client.Abstractions.Layers;
using DasCleverle.Mapbox.Layers;

namespace DasCleverle.DcsExport.LiveMap.Client;

internal class LayerRegistry : ILayerRegistry
{
    private List<ILayer> _cache = new();
    private bool _isInitialized = false;
    private object _syncRoot = new();

    private readonly IEnumerable<ILayerProvider> _providers;
    private readonly IEnumerable<ILayerExtender> _extenders;

    public LayerRegistry(IEnumerable<ILayerProvider> providers, IEnumerable<ILayerExtender> extenders)
    {
        _providers = providers;
        _extenders = extenders;
    }

    public IEnumerable<ILayer> GetLayers()
    {
        Initialize();
        return _cache;
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

            foreach (var provider in _providers)
            {
                var layers = provider.GetLayers();

                foreach (var layer in layers)
                {
                    var extenders = _extenders.Where(x => x.CanExtend(layer));
                    var extendedLayer = layer;

                    foreach (var extender in extenders)
                    {
                        extendedLayer = extender.Extend(extendedLayer);
                    }

                    _cache.Add(extendedLayer);
                }
            }

            _isInitialized = true;
        }
    }

}
