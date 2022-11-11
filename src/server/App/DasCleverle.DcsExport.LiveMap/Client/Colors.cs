using System.Drawing;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.Client;

internal static class IconColors
{
    private static readonly Dictionary<(Coalition, bool), Color> Map = new()
    {
        [(Coalition.Blue, false)] = Color.FromArgb(0xff, 0x91, 0xcc, 0xe3),
        [(Coalition.Blue, true)] = Color.FromArgb(0xff, 0x26, 0x8c, 0xb8),

        [(Coalition.Red, false)] = Color.FromArgb(0xff, 0xe9, 0x81, 0x7d),
        [(Coalition.Red, true)] = Color.FromArgb(0xff, 0xd9, 0x2e, 0x23),

        [(Coalition.Neutral, false)] = Color.FromArgb(0xff, 0xab, 0xd7, 0xa2)
    };

    public static Color? GetCoalitionColor(Coalition coalition, bool isPlayer)
    {
        return Map.TryGetValue((coalition, isPlayer), out var color) ? color : null;
    }
}
