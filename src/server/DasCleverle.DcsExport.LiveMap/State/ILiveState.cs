using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State
{
    public interface ILiveState
    {
        bool IsRunning { get; }

        ICollection<AddObjectPayload> Objects { get; }

        ICollection<AddAirbasePayload> Airbases { get; }

        string MissionName { get; }

        string Theatre { get; }

        Position MapCenter { get; }

        DateTime Time { get; }
    }

    public interface IWriteableLiveState 
    {
        bool IsRunning { get; set; }

        ConcurrentDictionary<int, AddObjectPayload> Objects { get; }

        ConcurrentDictionary<string, AddAirbasePayload> Airbases { get; }

        string MissionName { get; set; }

        string Theatre { get; set; }

        Position MapCenter { get; set; }

        DateTime Time { get; set; }
    }
}