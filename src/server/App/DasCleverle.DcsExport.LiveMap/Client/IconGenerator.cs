using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DasCleverle.DcsExport.Client.Icons;
using DasCleverle.DcsExport.Extensibility;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.LiveMap.Caching;
using Microsoft.Extensions.FileProviders;
using Svg;

namespace DasCleverle.DcsExport.LiveMap.Client;

internal class IconGenerator : IIconGenerator
{
    private readonly ICache _cache;
    private readonly IFileProvider _fileProvider;
    private readonly IExtensionManager _extensionManager;
    private readonly IconTemplateMapManager _iconTemplateMapManager;

    public IconGenerator(ICache cache, IWebHostEnvironment environment, IExtensionManager extensionManager, IconTemplateMapManager iconTemplateMapManager)
    {
        _cache = cache;
        _fileProvider = environment.WebRootFileProvider;
        _extensionManager = extensionManager;
        _iconTemplateMapManager = iconTemplateMapManager;
    }

    public IconKey GetIconKey(Coalition coalition, string typeName, IEnumerable<string> attributes, bool isPlayer)
    {
        var relevant = IconAttributeGraph.GetRelevantAttributes(attributes);
        return new IconKey(GetTemplates(typeName, relevant), coalition, isPlayer);
    }

    public Stream GenerateIcon(IconKey key)
    {
        var bytes = _cache.GetOrCreate<byte[]>(new IconCacheKey(key.ToString()), (entry) =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

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

            return RenderImage(svg);
        });

        return new MemoryStream(bytes, false);
    }

    private byte[] RenderImage(SvgDocument svg)
    {
        const float scale = 0.1875f;
        const int width = 128;
        const int height = 128;

        using var bitmap = new Bitmap(width, height);
        using var renderer = SvgRenderer.FromImage(bitmap);
        svg.RenderElement(renderer);

        var scaledWidth = (int)(width * scale);
        var scaledHeight = (int)(height * scale);

        using var scaled = new Bitmap(scaledWidth, scaledHeight);
        using var graphics = Graphics.FromImage(scaled);

        graphics.InterpolationMode = InterpolationMode.High;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.DrawImage(bitmap, new Rectangle(0, 0, scaledWidth, scaledHeight));

        using var stream = new MemoryStream();
        scaled.Save(stream, ImageFormat.Png);
        stream.Position = 0;

        var bytes = new byte[stream.Length];
        stream.Read(bytes, 0, (int)stream.Length);

        return bytes;
    }

    private IEnumerable<string> GetTemplates(string typeName, IEnumerable<string> attributes)
    {
        return _cache.GetOrCreate<HashSet<string>>(new TemplateCacheKey(typeName), (entry) =>
        {
            var map = _iconTemplateMapManager.GetTemplateMap();

            map.TypeNames.TryGetValue(typeName, out var typeNameMap);

            if (typeNameMap != null && typeNameMap.Replace != null)
            {
                return new HashSet<string>(typeNameMap.Replace);
            }

            var templates = new HashSet<string>();
            var supersessions = new HashSet<string>();

            foreach (var attribute in attributes)
            {
                if (!map.Attributes.TryGetValue(attribute, out var attributeMap))
                {
                    continue;
                }

                foreach (var template in attributeMap.Templates)
                {
                    templates.Add(template);
                }

                foreach (var supersession in attributeMap.Superseeds)
                {
                    supersessions.Add(supersession);
                }
            }

            foreach (var supersession in supersessions)
            {
                templates.Remove(supersession);
            }

            if (typeNameMap?.Remove != null)
            {
                foreach (var template in typeNameMap.Remove)
                {
                    templates.Remove(template);
                }
            }

            if (typeNameMap?.Add != null)
            {
                foreach (var template in typeNameMap.Add)
                {
                    templates.Add(template);
                }
            }

            if (templates.Count == 0)
            {
                templates.Add(map.Fallback);
            }

            return templates;
        });
    }

    private IFileInfo GetTemplateFile(Coalition coalition, string template)
    {
        var coalitionPath = Path.Combine("icons", IconKey.GetCoalitionName(coalition), $"{template}.svg");
        var commonPath = Path.Combine("icons", "common", $"{template}.svg");

        var extensionCoalitionFile = _extensionManager.GetAssetFile(coalitionPath);

        if (extensionCoalitionFile.Exists)
        {
            return extensionCoalitionFile;
        }

        var extensionFallbackFile = _extensionManager.GetAssetFile(commonPath);

        if (extensionFallbackFile.Exists)
        {
            return extensionFallbackFile;
        }

        var coalitionFile = _fileProvider.GetFileInfo(coalitionPath);

        if (coalitionFile.Exists)
        {
            return coalitionFile;
        }

        var fallbackFile = _fileProvider.GetFileInfo(commonPath);

        if (fallbackFile.Exists)
        {
            return fallbackFile;
        }

        throw new FileNotFoundException($"Could not find file for template '{template}'");
    }

    private record struct IconCacheKey
    {
        public string Key { get; }

        public IconCacheKey(string key)
        {
            Key = key;
        }
    }

    private record struct TemplateCacheKey
    {
        public string Key { get; }

        public TemplateCacheKey(string typeName)
        {
            Key = typeName;
        }
    }
}
