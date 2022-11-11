using DasCleverle.DcsExport.Client.Abstractions.Layers;
using DasCleverle.DcsExport.LiveMap.Abstractions;
using DasCleverle.Mapbox.Expressions;
using DasCleverle.Mapbox.Layers;

namespace DasCleverle.DcsExport.LiveMap.Client;

public class DefaultLayerProvider : ILayerProvider
{
    public IEnumerable<ILayer> GetLayers()
    {
        return new[]
        {
            new SymbolLayer
            {
                Id = Layers.Objects,
                Layout = new()
                {
                    IconImage = "{icon}",
                    IconAllowOverlap = true,
                    IconSize = Expression.Function<double>("coalesce", Expression.Function<double>("get", "iconSize"), 0.25),
                    SymbolSortKey = Expression.Function<double>("get", "sortKey"),
                }
            },
            new SymbolLayer
            {
                Id = Layers.Airbases,
                Layout = new()
                {
                    IconImage = "{icon}",
                    IconAllowOverlap = true,
                    IconSize = 0.175,
                    IconRotate = Expression.Function<double>("get", "rotation"),
                    TextField = "{name}",
                    TextAnchor = Anchor.Left,
                    TextOffset = new [] { 0.73, 0 },
                    TextFont = new [] { "DIN Pro Regular" },
                    TextSize = Expression.Function<double>(
                        "interpolate",
                        Expression.Function<double>("cubic-bezier", 0.2, 0, 0.9, 1),
                        Expression.Function<double>("zoom"),
                        3,
                        12,
                        13,
                        19
                    )
                },
                Paint = new()
                {
                    TextHaloColor = "white",
                    TextHaloWidth = 1,
                    TextHaloBlur = 1
                }
            }
        };
    }
}
