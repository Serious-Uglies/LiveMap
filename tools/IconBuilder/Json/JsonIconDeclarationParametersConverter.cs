using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IconBuilder.Json;

public class JsonIconDeclarationParametersConverter : JsonConverter<IEnumerable<Dictionary<string, string>>>
{
    public override IEnumerable<Dictionary<string, string>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.StartObject:
                return new[] { ReadDictionary(ref reader, options) };

            case JsonTokenType.StartArray:
                List<Dictionary<string, string>>? list = null;
                List<List<Dictionary<string, string>>>? matrix = null;

                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.StartObject:
                            if (matrix != null)
                            {
                                throw new JsonException("Already encountered an array before. Cannot mix arrays and object.");
                            }

                            if (list == null) 
                            {
                                list = new();
                            }

                            list.Add(ReadDictionary(ref reader, options));
                            break;

                        case JsonTokenType.StartArray:
                            if (list != null)
                            {
                                throw new JsonException("Already encountered an object before. Cannot mix arrays and object.");
                            }

                            if (matrix == null)
                            {
                                matrix = new();
                            }

                            var vector = new List<Dictionary<string, string>>();

                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType != JsonTokenType.StartObject)
                                {
                                    continue;
                                }

                                vector.Add(ReadDictionary(ref reader, options));
                            }

                            matrix.Add(vector);

                            break;

                        default:
                            throw new JsonException($"Expected either an array of arrays or an array of objects, but found '{reader.TokenType}'.");
                    }
                }

                if (list != null)
                {
                    return list;
                }

                if (matrix != null)
                {
                    var result = new List<Dictionary<string, string>>();
                    var keys = GetMatrixKeys(matrix);

                    for (int i = 0; i < keys.Length; i++)
                    {
                        var key = keys[i];
                        var values = new Dictionary<string, string>();

                        for (int d = 0; d < key.Length; d++)
                        {
                            Assign(values, matrix[d][key[d]]);
                        }

                        result.Add(values);
                    }

                    return result;
                }

                return new List<Dictionary<string, string>>();

            default:
                throw new JsonException("Expected an array or object.");
        }
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<Dictionary<string, string>> value, JsonSerializerOptions options)
        => throw new NotImplementedException();

    private static Dictionary<string, string> ReadDictionary(ref Utf8JsonReader reader, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader, options)!;

    private static int[][] GetMatrixKeys(List<List<Dictionary<string, string>>> matrix)
    {
        var totalLength = matrix.Aggregate(1, (total, vector) => total * vector.Count);
        var keys = new int[totalLength][];

        var prevSectionSize = totalLength;

        for (int d = 0; d < matrix.Count; d++)
        {
            var length = matrix[d].Count;
            var sectionSize = prevSectionSize / length;
            var sectionCount = totalLength / sectionSize;
            var k = 0;

            for (int c = 0; c < sectionCount; c++)
            {
                if (k == length) k = 0;

                for (int i = c * sectionSize; i < (c + 1) * sectionSize; i++)
                {
                    if (keys[i] == null)
                    {
                        keys[i] = new int[matrix.Count];
                    }

                    keys[i][d] = k;
                }

                k++;
            }

            prevSectionSize = sectionSize;
        }

        return keys;
    }

    private static void Assign(Dictionary<string, string> target, Dictionary<string, string>? source)
    {
        if (source == null)
        {
            return;
        }

        foreach (var (key, value) in source)
        {
            target[key] = value;
        }
    }

    private static Span<T> AsSpan<T>(T[] array, int start, int length)
    {
        return new Span<T>(array, start, length);
    }

}
