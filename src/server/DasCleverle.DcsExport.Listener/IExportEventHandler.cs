using System.Threading;
using System.Threading.Tasks;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.Listener
{
    public interface IExportEventHandler
    {
        Task HandleEventAsync(IExportEvent exportEvent, CancellationToken token);
    }
}