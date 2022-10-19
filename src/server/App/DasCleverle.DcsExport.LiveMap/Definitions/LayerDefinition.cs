using DasCleverle.DcsExport.Mapbox.Expressions;
using DasCleverle.DcsExport.Mapbox.Layers;

namespace DasCleverle.DcsExport.LiveMap.Definitions;

public static class LayerDefinition
{
    public static readonly ILayer[] Layers =
    {
        new SymbolLayer
        {
            Id = "objects",
            Layout = new() 
            {
                IconImage = "{icon}",
                IconAllowOverlap = true,
                IconSize = 0.7,
                SymbolSortKey = Expression.Function<double>("get", "sortKey"),
            }
        },
        new SymbolLayer 
        {
            Id = "airbases",
            Layout = new() 
            {
                IconImage = "{icon}",
                IconAllowOverlap = true,
                IconSize = 0.7,
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
