using System.Collections;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using DasCleverle.GeoJson.Json;

namespace DasCleverle.GeoJson;

/// <summary>   
/// Represents a <see href="https://www.rfc-editor.org/rfc/rfc7946#section-3.1.6">GeoJSON Polygon</see>.
/// </summary>
[JsonConverter(typeof(JsonGeometryConverter))]
public record Polygon : IGeometry<ImmutableList<LinearRing>>, IEnumerable<LinearRing>
{
    /// <summary>   
    /// Gets the type of the GeoJSON object (<see cref="GeoJsonType.Polygon" /> for Polygon).
    /// </summary>
    public GeoJsonType Type => GeoJsonType.Polygon;

    /// <summary>   
    /// Gets the linear rings defining the Polygon.
    /// </summary>
    public ImmutableList<LinearRing> Coordinates { get; init; } = ImmutableList<LinearRing>.Empty;

    internal Polygon() {}

    internal Polygon(IEnumerable<Position> ring) 
    {
        Coordinates = ImmutableList.Create(new LinearRing(ring));
    }

    internal Polygon(IEnumerable<IEnumerable<Position>> rings) 
    {
        Coordinates = ImmutableList.CreateRange(rings.Select(r => new LinearRing(r)));
    }

    internal Polygon(LinearRing ring) 
    {
        Coordinates = ImmutableList.Create(ring);
    }

    internal Polygon(IEnumerable<LinearRing> rings) 
    {
        Coordinates = ImmutableList.CreateRange(rings);
    }

    /// <summary>   
    /// Returns an enumerator that iterates through the linear rings of the Polygon.
    /// </summary>
    public IEnumerator<LinearRing> GetEnumerator() => Coordinates.GetEnumerator();

    /// <summary>   
    /// Returns an enumerator that iterates through the linear rings of the Polygon.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => Coordinates.GetEnumerator();
}

/// <summary>   
/// Represents a linear ring, the fundamental structure used to define a <see cref="Polygon" />.
/// </summary>
public record LinearRing : IEnumerable<Position>
{
    private ImmutableList<Position> _positions;

    internal LinearRing(IEnumerable<Position> positions)
    {
        _positions = ImmutableList.CreateRange(positions);
    }

    internal LinearRing(params Position[] positions)
    {
        _positions = ImmutableList.Create(positions);
    }

    /// <summary>   
    /// Returns an enumerator that iterates through the positions of the linear ring.
    /// </summary>
    public IEnumerator<Position> GetEnumerator() => _positions.GetEnumerator();

    /// <summary>   
    /// Returns an enumerator that iterates through the positions of the linear ring.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => _positions.GetEnumerator();
}
