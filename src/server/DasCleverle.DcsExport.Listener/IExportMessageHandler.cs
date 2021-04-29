using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Listener
{
    public interface IExportMessageHandler
    {
        Task<IExportEvent> HandleMessageAsync(ReadOnlySequence<byte> message, CancellationToken token);
    }
}

