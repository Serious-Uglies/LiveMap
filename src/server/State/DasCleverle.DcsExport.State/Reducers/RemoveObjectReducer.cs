using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.Listener.Model;
using DasCleverle.DcsExport.LiveMap.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State.Reducers;

public class RemoveObjectReducer : Reducer<RemoveObjectPayload>
{
    protected override LiveState Reduce(LiveState state, IExportEvent<RemoveObjectPayload> exportEvent)
    {
        return state.RemoveMapFeature(Layers.Objects, exportEvent.Payload.Id);
    }
}