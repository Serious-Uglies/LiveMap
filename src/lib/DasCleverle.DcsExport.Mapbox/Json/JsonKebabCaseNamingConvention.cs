using System.Text;
using System.Text.Json;

namespace DasCleverle.DcsExport.Mapbox.Json;

public class JsonKebabCaseNamingPolicy : JsonNamingPolicy
{
    public static readonly JsonKebabCaseNamingPolicy Instance = new();

    public override string ConvertName(string name)
    {
        var builder = new StringBuilder();

        for (int i = 0; i < name.Length; i++)
        {
            var c = name[i];

            if (i == 0 && char.IsUpper(c))
            {
                builder.Append(char.ToLower(c));
            }
            else if (char.IsUpper(c))
            {
                builder.Append('-').Append(char.ToLower(c));
            }
            else
            {
                builder.Append(c);
            }
        }

        return builder.ToString();
    }
}


public class JsonKebabCaseStringEnumConverterAttribute : JsonStringEnumWithNamingPolicyConverterAttribute
{
    public JsonKebabCaseStringEnumConverterAttribute() : base(JsonKebabCaseNamingPolicy.Instance) { }
}