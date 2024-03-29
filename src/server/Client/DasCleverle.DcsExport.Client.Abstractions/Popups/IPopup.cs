using System.Text.Json.Serialization;

namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

[JsonConverter(typeof(JsonPopupConverter))]
public interface IPopup
{
    string Type { get; }

    int Priority { get; }

    bool AllowClustering { get; }
}