# GeoJSON

This package attempts to implement the [GeoJSON specification](https://geojson.org/) in C# for .NET 5 and higher.
The main focus of this package is immutability.
All GeoJSON objects are [records](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/records) and collections
use the `ImmutableList<T>` type from the [System.Collections.Immutable](https://www.nuget.org/packages/System.Collections.Immutable) package.
This package also includes converters for the [System.Text.Json](https://www.nuget.org/packages/System.Text.Json) package.

## Usage

The package exposes the `GeoJSON` static class that contains constructor methods for all supported GeoJSON objects. 

Example:

```csharp
using DasCleverle.GeoJson;

var nullIsland = GeoJSON.Point(0, 0);
var myNextHike = GeoJSON.LineString((9.249188, 47.650219), (9.425973, 47.64395), (9.519018, 47.569409), (9.480158, 47.516946));
```

You can also add a `using static` to the top of the file to simplify:

```csharp
using static DasCleverle.GeoJson.GeoJSON;

var nullIsland = Point(0, 0);
var myNextHike = LineString((9.249188, 47.650219), (9.425973, 47.64395), (9.519018, 47.569409), (9.480158, 47.516946));
```

### A note about the `Position` struct

The `Position` struct can be converted from and to value tuples (as seen above).

```csharp
var myPosition = new Position(13.375167, 52.518600, 15);

// Destructure in into a separate values
var (longitude, latitude) = myPosition;

// Also the altitude value can be destructured
var (longitude, latitude, altitude) = myPosition;

// A method receiving a position as parameter ...
void DoSomethingWithPosition(Position position) {
    // ...
}

// ... can be called using a value tuple
DoSomethingWithPosition((13.375167, 52.518600, 200));
```

### Configuring the JSON serializer 

To get the best results when using the package, ensure that you the JsonSerializer is configured to use the camelCase property naming policy. 

```csharp
using static DasCleverle.GeoJson.GeoJSON;

var options = new JsonSerializerOptions 
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

var feature = Feature("platz-der-republik", Point(13.375167, 52.518600));
var json = JsonSerializer.Serialize(feature, options);
```

```json
{
    "type": "Feature",
    "id": "platz-der-republik",
    "geometry": {
        "type": "Point",
        "coordinates": [13.375167, 52.518600]
    }
}
``` 

### Using with ASP.NET Core

When using ASP.NET Core there is no need to manually configure the JSON serializer as the default is the camelCase naming policy.
This enables you to just return the GeoJSON object in your controllers.

```csharp
using static DasCleverle.GeoJson.GeoJSON;

[Route("api/[controller]")]
public class MyGeoController : Controller
{
    [HttpGet]
    public FeatureCollection Get()
    {
        var feature = Feature("platz-der-republik", Point(13.375167, 52.518600));
        return FeatureCollection(feature);
    }
}
```

Then the returned JSON will look something like this:

```json
[
    "type": "FeatureCollection",
    "features": [
        {
            "type": "Feature",
            "id": "platz-der-republik",
            "geometry": {
                "type": "Point",
                "coordinates": [13.375167, 52.518600]
            }
        }
    ]
]
```