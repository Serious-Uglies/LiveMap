using System.Drawing;
using System.Drawing.Imaging;
using IconBuilder.Model;
using Svg;

namespace IconBuilder;

public class IconRenderer
{
    private readonly DirectoryInfo _outputDirectory;

    public IconRenderer(DirectoryInfo outputDirectory)
    {
        _outputDirectory = outputDirectory;

        if (!outputDirectory.Exists)
        {
            outputDirectory.Create();
        }
    }

    public void RenderIcon(CompiledIconDeclaration icon)
    {
        var svg = new SvgDocument();

        ApplySources(svg, icon);
        ApplyStyles(svg, icon);

        var outputFile = new FileInfo(Path.Combine(_outputDirectory.FullName, $"{icon.Name}.png"));
        var size = GetSize(icon);

        using var bitmap = new Bitmap(size.Width, size.Height);
        using var renderer = SvgRenderer.FromImage(bitmap);
        svg.RenderElement(renderer);

        if (outputFile.Exists)
        {
            outputFile.Delete();
        }

        bitmap.Save(outputFile.FullName, ImageFormat.Png);
        Console.WriteLine($"Created icon '{outputFile.Name}' ({size.Width} x {size.Height})");
    }

    private void ApplySources(SvgDocument svg, CompiledIconDeclaration icon)
    {
        foreach (var source in icon.Sources)
        {
            var element = source.Svg.Children.Count == 1
                ? source.Svg.Children[0].DeepCopy()
                : CreateGroup(source.Svg.Children);

            if (element.Transforms == null)
            {
                element.Transforms = source.Transform;
            }
            else
            {
                element.Transforms.AddRange(source.Transform);
            }

            svg.Children.Add(element);
        }
    }

    private void ApplyStyles(SvgDocument svg, CompiledIconDeclaration icon)
    {
        foreach (var style in icon.Style)
        {
            var element = svg.Descendants().FirstOrDefault(x => x.ID == style.Id);

            if (element == null)
            {
                continue;
            }

            if (style.Fill != null)
            {
                element.Fill = new SvgColourServer(style.Fill.Value);
            }

            if (style.Stroke != null)
            {
                element.Stroke = new SvgColourServer(style.Stroke.Value);
            }
        }
    }

    private Size GetSize(CompiledIconDeclaration icon)
    {
        var width = icon.Sources.Max(x => (int)x.Svg.Width);
        var height = icon.Sources.Max(x => (int)x.Svg.Height);

        return new Size(width, height);
    }

    private SvgGroup CreateGroup(IEnumerable<SvgElement> elements)
    {
        var group = new SvgGroup();

        foreach (var element in elements)
        {
            group.Children.Add(element.DeepCopy());
        }

        return group;
    }
}
