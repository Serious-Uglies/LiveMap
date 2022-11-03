namespace DasCleverle.DcsExport.SampleExtension;

// This models the extension data added by the 'lua/sample.lua' script.
public record SampleExtensionData 
{
    public string ExtensionProperty { get; init; } = "";
}
