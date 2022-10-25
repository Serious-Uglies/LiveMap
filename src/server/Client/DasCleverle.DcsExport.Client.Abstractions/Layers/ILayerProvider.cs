using DasCleverle.Mapbox.Layers;

namespace DasCleverle.DcsExport.Client.Abstractions.Layers;

public interface ILayerProvider
{
    IEnumerable<ILayer> GetLayers();
}
