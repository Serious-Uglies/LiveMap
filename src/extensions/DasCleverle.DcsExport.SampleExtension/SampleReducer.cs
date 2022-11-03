using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.LiveMap.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.SampleExtension;

// A reducer receives events sent from DCS and maps them to a format understood by the LiveMap client.
// In this example we add the extension data we get from DCS to all map features on the 'airbases' layer.
public class SampleReducer : Reducer<AirbasePayload>
{
    // Determines the order reducers are run in. Higher numbers mean the given reducer is called later.
    // Here we specify an order of 1000 to make sure, our reducer is called very late in the chain.
    public override int Order => 1000;

    // This method is called by the core with the current state and the event that was raised in DCS.
    protected override LiveState Reduce(LiveState state, IExportEvent<AirbasePayload> exportEvent)
    {
        // Extract our extension data from the event payload. 
        // The key is the name of the LUA script file in which the data was appended.
        var extensionData = exportEvent.Payload.Extensions.Get<SampleExtensionData>("sample");

        // Do nothing if there is no extension data.
        // E.g. the extension is disabled on the DCS-side
        // or the data is only added conditionally.
        if (extensionData == null)
        {
            return state;
        }

        // Update the map feature on the airbase layer with the given ID.
        return state.UpdateMapFeature(Layers.Airbases, exportEvent.Payload.Id, feature => feature with
        {
            // Add our custom properties to the feature.
            // These properties can then be consumed by the map or popup.
            // See 'SamplePopupExtender.cs' for an example.
            Properties = feature.Properties.Add(new SampleProperties
            {
                SampleFeatureProperty = extensionData.ExtensionProperty 
            })
        });
    }
}
