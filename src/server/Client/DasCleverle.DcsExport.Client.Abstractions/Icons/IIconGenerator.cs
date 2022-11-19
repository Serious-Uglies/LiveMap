namespace DasCleverle.DcsExport.Client.Icons;

public interface IIconGenerator
{
    IconKey GetIconKey(string colorKey, string colorModifier, string typeName, IEnumerable<string> attributes);

    Stream GenerateIcon(IconKey key);
}
