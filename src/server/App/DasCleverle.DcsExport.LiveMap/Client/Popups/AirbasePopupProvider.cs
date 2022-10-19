using DasCleverle.DcsExport.Client.Abstractions.Popups;
using static DasCleverle.DcsExport.Client.Abstractions.Expressions.JexlExtensions;

namespace DasCleverle.DcsExport.LiveMap.Client.Popups;

public class AirbasePopupProvider : IPopupProvider
{
    public string Layer => "airbases";

    public IPopupBuilder GetPopup()
    {
        var builder = new PropertyListPopup.Builder();

        builder.Priority = 1;
        builder.AddRange(
            (
                "name",
                o => Translate("popup.airbase.name"),
                o => o["Name"]
            ),
            (
                "frequency",
                o => Translate("popup.airbase.frequency"),
                o => o["Frequencies"].Map(freq => Translate("format.frequency", new { Frequency = freq })).Join(", ")
            ),
            (
                "runways",
                o => Translate("popup.airbase.runways"),
                o => o["Runways"].Map(item => item["Edge1"] + "-" + item["Edge2"]).Join(", ")
            ),
            (
                "ils",
                o => Translate("popup.airbase.ils"),
                o => o["Beacons"]["ILS"].Map(ils => ils["Runway"] + ": " + Translate("format.beacon", ils)).Join(", ")
            ),
            (
                "tacan",
                o => Translate("popup.airbase.tacan"),
                o => o["Beacons"]["Tacan"].Map(tcn => Translate("format.tacan", tcn)).Join(", ")
            )
        );

        return builder;
    }
}