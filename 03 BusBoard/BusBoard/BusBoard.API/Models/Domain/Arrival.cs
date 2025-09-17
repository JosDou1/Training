using System.Text.Json.Serialization;

namespace BusBoard.Models.Domain;

public class Arrival
{
    [JsonPropertyName("lineName")] public required string LineName { get; set; }

    [JsonPropertyName("destinationName")] public required string DestinationName { get; set; }

    [JsonPropertyName("timeToStation")] public required int TimeToStationInSeconds { get; set; }

    public TimeSpan GetTimeToStation()
    {
        return TimeSpan.FromSeconds(TimeToStationInSeconds);
    }
}