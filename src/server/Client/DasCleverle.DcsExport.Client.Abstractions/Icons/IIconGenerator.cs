using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Client.Icons;

public interface IIconGenerator
{
    IconKey GetIconKey(Coalition coalition, string typeName, IEnumerable<string> attributes, bool isPlayer);

    Stream GenerateIcon(IconKey key);
}
