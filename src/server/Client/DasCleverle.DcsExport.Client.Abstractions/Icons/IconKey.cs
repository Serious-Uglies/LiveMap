namespace DasCleverle.DcsExport.Client.Icons;

public class IconKey 
{
    public string ColorKey { get; }

    public string ColorModifier { get; }

    public IEnumerable<string> Templates { get; }

    public IconKey(string colorKey, string colorModifier, IEnumerable<string> templates)
    {
        Templates = templates.ToArray();
        ColorKey = colorKey;
        ColorModifier = colorModifier;
    }

    public override string ToString()
    {
        var key = Templates
            .Prepend(ColorModifier)
            .Prepend(ColorKey);

        return string.Join(".", key);
    }

    public static IconKey Parse(string input)
    {
        var parts = input.Split('.');

        if (parts.Length < 3) 
        {
            throw new FormatException("An icon key requires at least three parts: '{color}.{colorModifier}.{...templates}'");
        }

        var colorKey = parts[0];
        var colorModifier = parts[1];
        var templates = parts[2..];

        return new IconKey(colorKey, colorModifier, templates);
    }
}

