using System.Collections.Immutable;
using DasCleverle.DcsExport.Client.Abstractions.Layers;
using DasCleverle.DcsExport.LiveMap.Caching;
using DasCleverle.Mapbox.Layers;

namespace DasCleverle.DcsExport.LiveMap.Client;

internal class LayerRegistry : ILayerRegistry
{
    private readonly object _cacheKey = new();
    private readonly ICache _cache;

    private readonly IEnumerable<ILayerProvider> _providers;
    private readonly IEnumerable<ILayerExtender> _extenders;

    public LayerRegistry(ICache cache, IEnumerable<ILayerProvider> providers, IEnumerable<ILayerExtender> extenders)
    {
        _cache = cache;
        _providers = providers;
        _extenders = extenders;
    }

    public IEnumerable<ILayer> GetLayers()
    {
        return _cache.GetOrCreate<ImmutableList<ILayer>>(_cacheKey, (entry) =>
        {
            var list = ImmutableList.CreateBuilder<ILayer>();

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

                    list.Add(extendedLayer);
                }

            }

            return list.ToImmutable();
        });
    }
}
