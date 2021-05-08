using System.Collections.Concurrent;
using System.Collections.Generic;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State
{
    public class LiveState : ILiveState, IWriteableLiveState
    {
        private ConcurrentDictionary<int, AddObjectPayload> _objects = new ConcurrentDictionary<int, AddObjectPayload>();
        private string _missionName;
        private string _theatre;
        private Position _mapCenter;

        ConcurrentDictionary<int, AddObjectPayload> IWriteableLiveState.Objects => _objects;

        string IWriteableLiveState.MissionName
        {
            get => _missionName;
            set => _missionName = value;
        }

        string IWriteableLiveState.Theatre
        {
            get => _theatre;
            set => _theatre = value;
        }

        Position IWriteableLiveState.MapCenter
        {
            get => _mapCenter; 
            set => _mapCenter = value;
        }

        public ICollection<AddObjectPayload> Objects => _objects.Values;

        public string MissionName => _missionName;

        public string Theatre => _theatre;

        public Position MapCenter => _mapCenter;
    }
}