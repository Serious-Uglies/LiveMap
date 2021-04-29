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

    public enum UnitCategory
    {
        Airplane = 0,
        Helicopter = 1,
        GroundUnit = 2,
        Ship = 3,
        Structure = 4
    }
}