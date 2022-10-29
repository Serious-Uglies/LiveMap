using DasCleverle.DcsExport.Client.Abstractions.Popups;
using DasCleverle.DcsExport.LiveMap.Abstractions;
using static DasCleverle.DcsExport.Client.Abstractions.Expressions.JexlExtensions;

namespace DasCleverle.DcsExport.LiveMap.Client.Popups;

public class ObjectPopupProvider : IPopupProvider
{
    public string Layer => "objects";

    public IPopupBuilder GetPopup()
    {
        return new GroupingPopup.Builder<ObjectProperties>()
            .WithGroupBy(o => o.Player != null ? o.Name + "-" + o.Player : o.Name)
            .WithRender(o => Translate(o.Value[0].Player != null ? "popup.object.player" : "popup.object.unit", new { Value = o.Value[0], Count = o.Value.Length }))
            .WithOrderBy(o => o.Value[0].Name);
    }
}