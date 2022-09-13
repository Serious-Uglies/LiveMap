using System.Buffers;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Listener;

public interface IExportMessageHandler
{
    Task<IExportEvent> HandleMessageAsync(ReadOnlySequence<byte> message, CancellationToken token);
}

