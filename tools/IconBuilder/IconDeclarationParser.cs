using System.Drawing;
using System.Text.Json;
using IconBuilder.Json;
using IconBuilder.Model;
using SmartFormat;
using Svg;
using Svg.Transforms;

namespace IconBuilder;

public class IconDeclarationParser
{
    public readonly FileInfo _definitionFile;
    public readonly DirectoryInfo _templateDirectory;

    public IconDeclarationParser(FileInfo definitionFile, DirectoryInfo templateDirectory)
    {
        _definitionFile = definitionFile;
        _templateDirectory = templateDirectory;
    }

    public IEnumerable<CompiledIconDeclaration> GetDeclarations()
    {
        var result = new List<CompiledIconDeclaration>();
        var definition = LoadDefinitionFile(_definitionFile);

        for (int d = 0; d < definition.Declarations.Length; d++)
        {
            IconDeclaration? declaration = definition.Declarations[d];
            var template = definition.Templates.FirstOrDefault(x => x.Id == declaration.Template);

            if (template == null)
            {
                throw new ParserException($"Could not find template '{declaration.Template}'.");
            }

            foreach (var declarationParameters in declaration.Parameters)
            {
                for (int v = 0; v < template.Variations.Length; v++)
                {
                    var variation = template.Variations[v];
                    var parameters = Merge(variation.Parameters, declarationParameters);
                    var context = new CompilationContext(definition, template, declaration, d, variation, v, parameters);

                    var compiled = CompileDeclaration(context);

                    result.Add(compiled);
                }
            }
        }

        return result;
    }

    private CompiledIconDeclaration CompileDeclaration(CompilationContext context)
    {
        var name = context.Evaluate(context.Template.Name);
        var sources = context.Template.Sources
            .Select((x, i) => CompileSource(context, x, i))
            .ToArray();

        var styles = context.Variation.Style
            .Select((s, i) => CompileIconStyle(context, s, i))
            .ToArray();

        return new CompiledIconDeclaration
        {
            Name = name,
            Sources = sources,
            Style = styles
        };
    }

    private CompiledTemplateSource CompileSource(CompilationContext context, TemplateSource source, int index)
    {
        var fileName = context.Evaluate(source.Name);
        var path = Path.Combine(_templateDirectory.FullName, fileName);

        if (!File.Exists(path))
        {
            throw new ParserException($"Could not find template source file '{path}' ({context.Path}, source index: {index}).");
        }

        SvgDocument svg;
        try
        {
            svg = SvgDocument.Open(path);
        }
        catch (Exception ex)
        {
            throw new ParserException($"Could not load template source file '{path}' ({context.Path}, source index: {index}).", ex);
        }

        var transform = SvgTransformConverter.Parse(context.Evaluate(source.Transform));

        return new CompiledTemplateSource(svg, transform);
    }

    private CompiledIconStyle CompileIconStyle(CompilationContext context, IconStyle style, int index)
    {
        return new CompiledIconStyle(
            context.Evaluate(style.Id),
            GetColor(context.Evaluate(style.Fill)),
            GetColor(context.Evaluate(style.Stroke))
        );

        Color? GetColor(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (!context.Definition.Colors.TryGetValue(name, out var color))
            {
                throw new ParserException($"Could not find color '{name}' ({context.Path}, style index: {index})");
            }

            return color;
        }
    }

    private static IconDefinition LoadDefinitionFile(FileInfo file)
    {
        try
        {
            using var stream = file.OpenRead();
            var definition = JsonSerializer.Deserialize<IconDefinition>(stream, GetOptions());

            if (definition == null)
            {
                throw new ParserException("Failed to load defintion file.");
            }

            return definition;
        }
        catch (Exception ex)
        {
            throw new ParserException("Failed to load defintion file.", ex);
        }
    }

    private static JsonSerializerOptions GetOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        options.Converters.Add(new JsonTemplateDeclarationConverter());
        options.Converters.Add(new JsonColorConverter());

        return options;
    }

    private static Dictionary<string, string> Merge(params Dictionary<string, string>[] dictionaries)
    {
        var result = new Dictionary<string, string>();

        foreach (var dictionary in dictionaries)
        {
            foreach (var (key, value) in dictionary)
            {
                result[key] = value;
            }
        }

        return result;
    }

    private record CompilationContext(
        IconDefinition Definition,
        IconTemplate Template,
        IconDeclaration Declaration,
        int DeclarationIndex,
        IconVariation Variation,
        int VariationIndex,
        Dictionary<string, string> Parameters
    )
    {
        public string Evaluate(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            try
            {
                return Smart.Format(input, Parameters);
            }
            catch (Exception ex)
            {
                throw new ParserException($"Failed to evaluate template string '{input}' ({Path}).", ex);
            }

        }

        public string Path => $"template: {Template.Id}, declaration index: {DeclarationIndex}, variation index: {VariationIndex}";
    }
}
