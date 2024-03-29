namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public interface IPopupRegistry
{
    IPopup? GetPopup(string layer);

    IDictionary<string, IPopup> GetPopups();
}
