namespace DasCleverle.DcsExport.Extensibility;

public class ExtensionLoaderException : Exception
{
    public ExtensionLoaderException()
    {
    }

    public ExtensionLoaderException(string id, string? message) : this(id, message, null)
    {
    }

    public ExtensionLoaderException(string id, string? message, Exception? innerException) : base($"{message} (Extension ID: {id})", innerException)
    {
    }
}