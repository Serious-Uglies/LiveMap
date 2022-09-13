using DasCleverle.DcsExport.Listener.Abstractions;
using DasCleverle.DcsExport.State.Abstractions;

namespace DasCleverle.DcsExport.State;

public class LiveStateEventHandler : IExportEventHandler
{
    private readonly ILiveStateStore _store;

    public LiveStateEventHandler(ILiveStateStore store)
    {
        _store = store;
    }

    public async Task HandleEventAsync(IExportEvent exportEvent, CancellationToken token)
    {
        await _store.DispatchAsync(exportEvent.Payload);
    }
}