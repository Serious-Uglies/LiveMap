namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public interface IPopupProvider
{
    string Layer { get; }

    IPopup GetPopup();
}
