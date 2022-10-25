using DasCleverle.Mapbox.Layers;

namespace DasCleverle.DcsExport.Client.Abstractions.Layers;

public interface ILayerExtender
{
    bool CanExtend(ILayer layer);

    ILayer Extend(ILayer layer);
}

public interface ILayerExtender<T> : ILayerExtender where T : ILayer
{
    T Extend(T layer);

    ILayer ILayerExtender.Extend(ILayer layer) => layer is T ? Extend((T)layer) : layer;
}