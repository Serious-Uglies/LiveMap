using System.Buffers;
using DasCleverle.DcsExport.Listener.Abstractions;

namespace DasCleverle.DcsExport.Listener;

public interface IMessageParser
{
    Task<IExportEvent> ParseMessageAsync(ReadOnlySequence<byte> message, CancellationToken token);
}

