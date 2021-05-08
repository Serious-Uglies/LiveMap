using System.Collections.Concurrent;
using System.Collections.Generic;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State
{
    public interface ILiveState
    {
        ICollection<AddObjectPayload> Objects { get; }

        string MissionName { get; }

        string Theatre { get; }

        Position MapCenter { get; }
    }

    public interface IWriteableLiveState 
    {
        ConcurrentDictionary<int, AddObjectPayload> Objects { get; }

        string MissionName { get; set; }

        string Theatre { get; set; }

        Position MapCenter { get; set; }
    }
}