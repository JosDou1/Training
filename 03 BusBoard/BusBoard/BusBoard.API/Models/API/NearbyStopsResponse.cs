using System.Text.Json.Serialization;
using BusBoard.Models.Domain;

namespace BusBoard.Models.API;

public class NearbyStopsResponse
{
    [JsonPropertyName("stopPoints")] public required List<StopPoint> StopPoints { get; set; }
}