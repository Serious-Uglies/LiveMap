using System.Drawing;
using System.Drawing.Imaging;
using DasCleverle.DcsExport.Client.Icons;
using DasCleverle.DcsExport.Listener.Model;
using Microsoft.Extensions.FileProviders;
using Svg;

namespace DasCleverle.DcsExport.LiveMap.Client;

internal class IconGenerator : IIconGenerator
{
    private readonly IFileProvider _fileProvider;

    public IconGenerator(IWebHostEnvironment environment)
    {
        _fileProvider = environment.WebRootFileProvider;
    }

    public IconKey GetIconKey(Coalition coalition, string typeName, IEnumerable<string> attributes, bool isPlayer)
    {
        var relevant = IconAttributeGraph.GetRelevantAttributes(attributes);
        return new IconKey(GetTemplates(coalition, typeName, relevant), coalition, isPlayer);
    }

    public Stream GenerateIcon(IconKey key)
    {
        var svg = new SvgDocument();
        var templates = key.Templates
            .Select(x => GetTemplateFile(key.Coalition, x))
            .Select(x => SvgDocument.Open(x.PhysicalPath))
            .ToArray();

        foreach (var template in templates)
        {
            foreach (var child in template.Children)
            {
                var resultChild = child;

                if (child.CustomAttributes.TryGetValue("colorable", out var colorable) && colorable == "true")
                {
                    var color = IconColors.GetCoalitionColor(key.Coalition, key.IsPlayer);

                    if (color.HasValue)
                    {
                        resultChild = child.DeepCopy();
                        resultChild.Fill = new SvgColourServer(color.Value);
                    }
                }

                svg.Children.Add(resultChild);
            }
        }

        using var bitmap = new Bitmap(128, 128);
        using var renderer = SvgRenderer.FromImage(bitmap);
        svg.RenderElement(renderer);

        var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        stream.Position = 0;

        return stream;
    }

    private IEnumerable<string> GetTemplates(Coalition coalition, string typeName, IEnumerable<string> attributes)
    {
        var templates = new HashSet<string>();

        if (IconTemplateMap.TypeNameMap.TryGetValue(typeName, out var typeNameTemplates))
        {
            foreach (var name in typeNameTemplates)
            {
                templates.Add(name);
            }
        }
        else
        {
            foreach (var attribute in attributes)
            {
                if (!IconTemplateMap.AttributeMap.TryGetValue(attribute, out var attributeTemplates))
                {
                    continue;
                }

                foreach (var name in attributeTemplates)
                {
                    templates.Add(name);
                }
            }
        }

        if (templates.Count == 0)
        {
            templates.Add("ground");
        }

        return templates;
    }

    private IFileInfo GetTemplateFile(Coalition coalition, string template)
    {
        var coalitionPath = Path.Combine("icons", IconKey.GetCoalitionName(coalition), $"{template}.svg");
        var coalitionFile = _fileProvider.GetFileInfo(coalitionPath);

        if (coalitionFile.Exists)
        {
            return coalitionFile;
        }

        var commonPath = Path.Combine("icons", "common", $"{template}.svg");
        var fallbackFile = _fileProvider.GetFileInfo(commonPath);

        if (fallbackFile.Exists)
        {
            return fallbackFile;
        }

        throw new FileNotFoundException($"Could not find file for template '{template}'");
    }
}
