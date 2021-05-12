using System.Collections.Generic;
using System.ComponentModel;

namespace DasCleverle.DcsExport.Listener.Model
{
    public enum EventType
    {
        [EventPayload(typeof(InitPayload))]
        Init,

        [EventPayload(typeof(MissionEndPayload))]
        MissionEnd,

        [EventPayload(typeof(AddObjectPayload))]
        AddObject,

        [EventPayload(typeof(RemoveObjectPayload))]
        RemoveObject,

        [EventPayload(typeof(UpdateObjectPayload))]
        UpdateObject,

        [EventPayload(typeof(AddAirbasePayload))]
        AddAirbase
    }

    public enum Coalition
    {
        Neutral = 0,
        Red = 1,
        Blue = 2
    }

    public enum ObjectAttribute
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

    public enum ObjectType
    {
        Unknown = 0,
        Unit = 1,
        Static = 2
    }
}