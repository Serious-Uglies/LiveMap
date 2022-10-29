using System.Text;
using System.Text.Json;

namespace DasCleverle.Mapbox.Json;

internal class JsonKebabCaseNamingPolicy : JsonNamingPolicy
{
    public static readonly JsonKebabCaseNamingPolicy Instance = new();

    public override string ConvertName(string name)
    {
        var length = 0;

        for (int i = 0; i < name.Length; i++)
        {
            length++;

            if (i != 0 && char.IsUpper(name[i]))
            {
                length++;
            }
        }

        return string.Create(length, name, (chars, name) =>
        {
            int ci = 0;
            for (int i = 0; i < name.Length; i++)
            {
                var c = name[i];

                if (i == 0 && char.IsUpper(c))
                {
                    chars[ci] = char.ToLower(c);
                    ci++;
                }
                else if (char.IsUpper(c))
                {
                    chars[ci] = '-';
                    ci++;
                    
                    chars[ci] = char.ToLower(c);
                    ci++;
                }
                else 
                {
                    chars[ci] = c;
                    ci++;
                }
            }
        });
    }
}

internal class JsonStringStringEnumKebabCaseConverterAttribute : JsonStringEnumNamingPolicyConverterAttribute
{
    public JsonStringStringEnumKebabCaseConverterAttribute() : base(JsonKebabCaseNamingPolicy.Instance) { }
}