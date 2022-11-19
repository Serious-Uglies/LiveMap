using System.Collections;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

namespace DasCleverle.DcsExport.Extensibility;

internal class ExtensionAssetFileProvider : IFileProvider
{
    private static readonly IEqualityComparer<string?> Comparer = OperatingSystem.IsWindows()
        ? StringComparer.OrdinalIgnoreCase
        : StringComparer.Ordinal;

    private readonly IEnumerable<Extension> _extensions;

    public ExtensionAssetFileProvider(IEnumerable<Extension> extensions)
    {
        _extensions = extensions;
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var contents = _extensions
            .Select(x => new DirectoryInfo(Path.Combine(x.BaseDirectory.FullName, "assets", subpath)))
            .Where(x => x.Exists)
            .SelectMany(x => x.EnumerateFileSystemInfos())
            .Join(GetAssetFiles(), f => f.FullName, p => p, (f, p) => f, Comparer);

        return new ExtensionDirectoryContents(contents);
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        var assetFiles = GetAssetFiles();

        foreach (var extension in _extensions)
        {
            var file = new FileInfo(Path.GetFullPath(Path.Combine(extension.BaseDirectory.FullName, "assets", subpath)));

            foreach (var asset in extension.Assets)
            {
                if (!Comparer.Equals(file.FullName, asset.FullName)) 
                {
                    continue;
                }

                return new PhysicalFileInfo(asset);
            }
        }

        // Do something to generate a non-existant file path
        return new PhysicalFileInfo(new FileInfo($"{Guid.NewGuid()}_{subpath}"));
    }

    public IChangeToken Watch(string filter) => throw new NotSupportedException();

    private IEnumerable<string> GetAssetFiles() => _extensions.SelectMany(x => x.Assets).Select(x => x.FullName);

    private class ExtensionDirectoryContents : IDirectoryContents
    {
        private readonly IEnumerable<IFileInfo> _contents;

        public bool Exists => true;

        public ExtensionDirectoryContents(IEnumerable<FileSystemInfo> files)
        {
            _contents = files.Select(x => (IFileInfo)(x switch 
            {
                FileInfo f => new PhysicalFileInfo(f),
                DirectoryInfo d => new PhysicalDirectoryInfo(d),
                _ => throw new InvalidOperationException($"Unkown file system info type {x.GetType()}.")
            }));
        }

        public IEnumerator<IFileInfo> GetEnumerator() => _contents.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _contents.GetEnumerator();
    }
}
