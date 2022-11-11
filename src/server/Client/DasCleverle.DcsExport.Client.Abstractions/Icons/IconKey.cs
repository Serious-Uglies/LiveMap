using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Client.Icons;

public class IconKey 
{
    public IEnumerable<string> Templates { get; }

    public Coalition Coalition { get; }

    public bool IsPlayer { get; }

    public IconKey(IEnumerable<string> templates, Coalition coalition, bool isPlayer)
    {
        Templates = templates.ToArray();
        Coalition = coalition;
        IsPlayer = isPlayer;
    }

    public override string ToString()
    {
        return string.Join(".", Templates
            .Prepend(GetCoalitionName(Coalition))
            .Append(IsPlayer ? "player" : "ai"));
    }

    public static IconKey Parse(string input)
    {
        var parts = input.Split('.');

        if (parts.Length < 3) 
        {
            throw new FormatException();
        }

        var coalitionName = parts[0];
        var templates = parts[1..^1];
        var controller = parts[^1];

        if (!Enum.TryParse<Coalition>(coalitionName, ignoreCase: true, out var coalition))
        {
            throw new FormatException();
        }

        var isPlayer = controller == "player";
        var isAi = controller == "ai";

        if (!isPlayer && !isAi)
        {
            throw new FormatException();
        }

        return new IconKey(templates, coalition, isPlayer);
    }

    public static string GetCoalitionName(Coalition coalition)
        => coalition.ToString().ToLowerInvariant();

}

