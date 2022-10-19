using DasCleverle.DcsExport.Client.Abstractions.Popups;
using static DasCleverle.DcsExport.Client.Abstractions.Expressions.JexlExtensions;

namespace DasCleverle.DcsExport.LiveMap.Client.Popups;

public class ObjectPopupProvider : IPopupProvider
{
    public string Layer => "objects";

    public IPopupBuilder GetPopup()
    {
        return new GroupingPopup.Builder
        {
            GroupBy = (o) => o["Player"] ? o["Name"] + "-" + o["Player"] : o["Name"],
            Render = (o) => Translate(o["Value"][0]["Player"] ? "popup.object.player" : "popup.object.unit", new { Value = o["Value"][0], Count = o["Count"] }),
            OrderBy = (o) => o["Value"][0]["Name"]
        };
    }
}

public interface IObjectProperties
{
    string Name { get; }

    string Player { get; }

    int? Frequency { get; }
}
