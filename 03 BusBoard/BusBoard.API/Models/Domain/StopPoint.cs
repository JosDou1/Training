using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace BusBoard.Models.Domain;

public class StopPoint
{
    [JsonPropertyName("naptanId")] public required string Id { get; set; }

    [JsonPropertyName("commonName")] public required string Name { get; set; }

    [JsonPropertyName("distance")] public required float Distance { get; set; }

    [JsonPropertyName("lat")] public required float Latitude { get; set; }

    [JsonPropertyName("lon")] public required float Longitude { get; set; }

    [JsonIgnore] public ImmutableList<Arrival> Arrivals { get; set; } = [];
}