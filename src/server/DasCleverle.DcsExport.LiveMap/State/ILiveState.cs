using System.Collections.Concurrent;
using System.Collections.Generic;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State
{
    public interface ILiveState
    {
        public ICollection<UnitPayload> Units { get; }
    }

    public interface IWriteableLiveState 
    {
        public ConcurrentDictionary<int, UnitPayload> Units { get; }
    }
}