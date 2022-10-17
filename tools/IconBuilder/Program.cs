using System.CommandLine;
using System.CommandLine.Parsing;
using IconBuilder;
using IconBuilder.Model;

var rootCommand = new RootCommand("IconBuilder");

var definitionOption = new Option<FileInfo?>(
    aliases: new[] { "--definition", "-d" },
    parseArgument: ParseDefinitionOption,
    description: "A path to file containing the icon definitions."
) { IsRequired = true };

var templatesOption = new Option<DirectoryInfo?>(
    aliases: new[] { "--template", "-t" },
    parseArgument: ParseTemplateOption,
    description: "A path to a directory where the templates to use are located at."
) { IsRequired = true };

var outputOption = new Option<DirectoryInfo>(
    aliases: new[] { "--output", "-o" },
    description: "A path to a directory to output the generated icons to."
) { IsRequired = true };

rootCommand.Add(definitionOption);
rootCommand.Add(templatesOption);
rootCommand.Add(outputOption);

rootCommand.SetHandler((definitionFile, templateDirectory, outputDirectory) =>
{
    if (definitionFile == null || templateDirectory == null)
    {
        return;
    }

    var parser = new IconDeclarationParser(definitionFile, templateDirectory);
    var renderer = new IconRenderer(outputDirectory);

    IEnumerable<CompiledIconDeclaration> declarations;

    try
    {
        declarations = parser.GetDeclarations();
    }
    catch (ParserException ex)
    {
        Console.Error.WriteLine(ex.Message);

        if (ex.InnerException != null)
        {
            Console.Error.WriteLine($"--> {ex.InnerException.Message}");
        }

        return;
    }

    foreach (var icon in declarations)
    {
        renderer.RenderIcon(icon);
    }
}, definitionOption, templatesOption, outputOption);

await rootCommand.InvokeAsync(args);



FileInfo? ParseDefinitionOption(ArgumentResult result) 
{
    var path = result.Tokens[0].Value;

    if (!string.Equals(Path.GetExtension(path), ".json", StringComparison.OrdinalIgnoreCase))
    {
        result.ErrorMessage = $"Invalid option '--{result.Argument.Name}': The file '{path}' is not a JSON file.";
        return null;
    }

    if (!File.Exists(path))
    {
        result.ErrorMessage = $"Invalid option '--{result.Argument.Name}': Could not find file '{path}'.";
        return null;
    }

    return new FileInfo(path);
}

DirectoryInfo? ParseTemplateOption(ArgumentResult result) 
{
    var path = result.Tokens[0].Value;

    if (!Directory.Exists(path))
    {
        result.ErrorMessage = $"Invalid option '--{result.Argument.Name}': Could not find directory '{path}'.";
        return null;
    }

    return new DirectoryInfo(path);
}
