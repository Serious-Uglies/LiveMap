using DasCleverle.DcsExport.Listener;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State.Handlers;

public class UpdateObjectHandler : IExportEventHandler<UpdateObjectPayload>
{
    private readonly IWriteableLiveState _state;

    public UpdateObjectHandler(IWriteableLiveState state)
    {
        _state = state;
    }

    public Task HandleEventAsync(IExportEvent<UpdateObjectPayload> exportEvent, CancellationToken token)
    {
        Handle(exportEvent, token);
        return Task.CompletedTask;
    }

    private void Handle(IExportEvent<UpdateObjectPayload> exportEvent, CancellationToken token)
    {
        var id = exportEvent.Payload.Id;
        var position = exportEvent.Payload.Position;

        var isUpdated = false;

        do
        {
            if (!_state.Objects.TryGetValue(id, out var obj))
            {
                return;
            }

            var updated = obj with
            {
                Position = position
            };

            isUpdated = _state.Objects.TryUpdate(id, updated, obj);
        } while (!isUpdated);
    }
}