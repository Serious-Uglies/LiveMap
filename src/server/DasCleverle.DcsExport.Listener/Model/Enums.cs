using System;
using System.ComponentModel;

namespace DasCleverle.DcsExport.Listener.Model
{
    public enum EventType
    {
        [EventPayload(typeof(InitPayload))]
        Init,

        [EventPayload(typeof(MissionEndPayload))]
        MissionEnd,

        [EventPayload(typeof(UnitPayload))]
        AddUnit,

        [EventPayload(typeof(RemoveUnitPayload))]
        RemoveUnit,

        [EventPayload(typeof(UpdatePositionPayload))]
        UpdatePosition
    }

    public enum Coalition
    {
        Neutral = 0,
        Red = 1,
        Blue = 2
    }

    public enum UnitAttribute
    {
        [Description("Planes")]
        Fixed,

        [Description("AWACS")]
        Awacs,

        [Description("Tankers")]
        Tanker,

        [Description("Helicopters")]
        Rotary,

        [Description("Ground Units")]
        Ground,

        [Description("Ships")]
        Water
    }
}