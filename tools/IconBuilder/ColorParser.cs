using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace IconBuilder;

public static class ColorParser
{
    private static readonly Regex Regex = new("#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})", RegexOptions.Compiled);

    public static Color Parse(string rgbHex)
    {
        var match = Regex.Match(rgbHex);

        if (!match.Success)
        {
            throw new FormatException("Expected a color hex string in the format '#rrggbb'.");
        }

        var red = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
        var green = int.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
        var blue = int.Parse(match.Groups[3].Value, NumberStyles.HexNumber);

        return Color.FromArgb(255, red, green, blue);
    }
}
