using System.Threading;
using System.Threading.Tasks;

namespace DasCleverle.DcsExport.Listener.Abstractions
{
    public interface IExportEventHandler
    {
        Task HandleEventAsync(IExportEvent exportEvent, CancellationToken token);
    }

    public interface IExportEventHandler<T> where T : IEventPayload
    {
        Task HandleEventAsync(IExportEvent<T> exportEvent, CancellationToken token);
    }
}