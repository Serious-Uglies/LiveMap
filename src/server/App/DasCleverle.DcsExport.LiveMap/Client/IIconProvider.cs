namespace DasCleverle.DcsExport.LiveMap.Client;

public interface IIconProvider
{
    IEnumerable<IconInfo> GetIcons();

    Stream? GetIconFile(string fileName);
}

public record IconInfo(string Id, string Url);