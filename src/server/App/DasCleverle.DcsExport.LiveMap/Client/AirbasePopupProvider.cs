using DasCleverle.DcsExport.Client.Abstractions.Expressions;
using DasCleverle.DcsExport.Client.Abstractions.Popups;
using DasCleverle.DcsExport.LiveMap.Abstractions;
using static DasCleverle.DcsExport.Client.Abstractions.Expressions.JexlExtensions;

namespace DasCleverle.DcsExport.LiveMap.Client;

public class AirbasePopupProvider : IPopupProvider
{
    public string Layer => Layers.Airbases;

    public IPopup GetPopup()
    {
        return new PropertyListPopup<AirbaseProperties>()
            .WithPriority(1)
            .AddScalar(
                "name",
                o => Translate("popup.airbase.name"),
                o => o.Name
            )
            .AddList(
                "frequencies",
                o => Translate("popup.airbase.frequency"),
                o => o.Frequencies,
                i => Translate("format.frequency", new { Frequency = i.Item })
            )
            .AddScalar(
                "tacan",
                o => Translate("popup.airbase.tacan"),
                o => o.Beacons.Tacan.Map(tacan => Translate("format.tacan", tacan)).Join(", ")
            )
            .AddList(
                "runways",
                o => Translate("popup.airbase.runways"),
                o => o.Runways,
                o => o.Item.Name
            )
            .AddList(
                "ils",
                o => Translate("popup.airbase.ils"),
                o => o.Beacons.ILS,
                o => o.Item.Runway + ": " + Translate("format.beacon", o.Item)
            );
    }
}