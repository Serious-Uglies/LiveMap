namespace DasCleverle.DcsExport.Client.Abstractions.Popups;

public interface IPopupExtender
{
    string Layer { get; }

    void Extend(IPopupBuilder popup);
}

public interface IPopupExtender<T> : IPopupExtender where T : IPopupBuilder
{
    void IPopupExtender.Extend(IPopupBuilder popup)
    {
        if (popup is not T typed)
        {
            return;
        }

        Extend(typed);
    }

    void Extend(T popup);
}
