using System.Collections;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;

namespace DasCleverle.DcsExport.Extensibility;

internal class ExtensionFileProvider : IFileProvider
{
    private static readonly IEqualityComparer<string?> Comparer = OperatingSystem.IsWindows()
        ? StringComparer.OrdinalIgnoreCase
        : StringComparer.Ordinal;

    private readonly IEnumerable<Extension> _extensions;

    public ExtensionFileProvider(IEnumerable<Extension> extensions)
    {
        _extensions = extensions;
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var contents = _extensions
            .SelectMany(x => new DirectoryInfo(Path.Combine(x.BaseDirectory.FullName, "assets", subpath)).EnumerateFiles())
            .Join(GetAssetFiles(), f => f.FullName, p => p, (f, p) => f, Comparer);

        return new ExtensionDirectoryContents(contents);
    }

    public IFileInfo GetFileInfo(string subpath) => throw new NotSupportedException();

    public IChangeToken Watch(string filter) => throw new NotSupportedException();

    private IEnumerable<string> GetAssetFiles() => _extensions.SelectMany(x => x.Assets).Select(x => x.FullName);

    private class ExtensionDirectoryContents : IDirectoryContents
    {
        private readonly IEnumerable<IFileInfo> _files;

        public bool Exists => true;

        public ExtensionDirectoryContents(IEnumerable<FileInfo> files)
        {
            _files = files.Select(x => new PhysicalFileInfo(x));
        }

        public IEnumerator<IFileInfo> GetEnumerator() => _files.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _files.GetEnumerator();
    }
}
