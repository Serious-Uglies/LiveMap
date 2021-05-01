using System.Collections.Concurrent;
using System.Collections.Generic;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State
{
    public class LiveState : ILiveState, IWriteableLiveState
    {
        private ConcurrentDictionary<int, UnitPayload> _units = new ConcurrentDictionary<int, UnitPayload>();

        ConcurrentDictionary<int, UnitPayload> IWriteableLiveState.Units => _units;

        public ICollection<UnitPayload> Units => _units.Values;
    }
}