using System.Buffers;
using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener;

public interface IMessageParser
{
    Task<IExportEvent> HandleMessageAsync(ReadOnlySequence<byte> message, CancellationToken token);
}

