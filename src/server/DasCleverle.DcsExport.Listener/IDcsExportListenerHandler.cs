using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace DasCleverle.DcsExport.Listener
{
    public interface IDcsExportListenerHandler
    {
        Task HandleMessageAsync(ReadOnlySequence<byte> message, CancellationToken token);
    }
}

