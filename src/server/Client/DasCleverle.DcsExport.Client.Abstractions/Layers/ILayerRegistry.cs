using DasCleverle.Mapbox.Layers;

namespace DasCleverle.DcsExport.Client.Abstractions.Layers;

public interface ILayerRegistry
{
    IEnumerable<ILayer> GetLayers();
}
