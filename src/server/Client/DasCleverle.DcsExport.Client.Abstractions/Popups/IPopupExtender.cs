namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public interface IPopupExtender
{
    string Layer { get; }

    IPopup Extend(IPopup popup);
}

public interface IPopupExtender<T> : IPopupExtender where T : IPopup
{
    IPopup IPopupExtender.Extend(IPopup popup)
    {
        if (popup is not T typed)
        {
            return popup;
        }

        return Extend(typed);
    }

    IPopup Extend(T popup);
}
