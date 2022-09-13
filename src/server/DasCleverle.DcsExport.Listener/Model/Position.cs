namespace DasCleverle.DcsExport.Listener.Model;

public record Position
{
    public decimal Lat { get; init; }

    public decimal Long { get; init; }
}