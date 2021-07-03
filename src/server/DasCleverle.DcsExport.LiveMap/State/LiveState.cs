using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DasCleverle.DcsExport.Listener.Model;

namespace DasCleverle.DcsExport.LiveMap.State
{
    public class LiveState : ILiveState, IWriteableLiveState
    {
        private bool _isRunning;
        private ConcurrentDictionary<int, AddObjectPayload> _objects = new();
        private ConcurrentDictionary<string, AddAirbasePayload> _airbases = new();
        private string _missionName;
        private string _theatre;
        private Position _mapCenter;
        private DateTime _time;

        ConcurrentDictionary<int, AddObjectPayload> IWriteableLiveState.Objects => _objects;

        ConcurrentDictionary<string, AddAirbasePayload> IWriteableLiveState.Airbases => _airbases;

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

        DateTime IWriteableLiveState.Time
        {
            get => _time;
            set => _time = value;
        }

        bool IWriteableLiveState.IsRunning
        {
            get => _isRunning;
            set => _isRunning = value;
        }

        public ICollection<AddObjectPayload> Objects => _objects.Values;

        public ICollection<AddAirbasePayload> Airbases => _airbases.Values;

        public string MissionName => _missionName;

        public string Theatre => _theatre;

        public Position MapCenter => _mapCenter;

        public DateTime Time => _time;

        public bool IsRunning => _isRunning;
    }
}