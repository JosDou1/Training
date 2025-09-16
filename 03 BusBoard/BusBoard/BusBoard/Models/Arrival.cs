using System.Text.Json.Serialization;

namespace BusBoard.Models;

public class Arrival
{
    [JsonPropertyName("lineName")] public required string LineName { get; set; }

    [JsonPropertyName("destinationName")] public required string DestinationName { get; set; }
    
    [JsonPropertyName("timeToStation")] public required int TimeToStationInSeconds { get; set; }

    [JsonIgnore]
    public TimeSpan GetTimeToStation
    {
        get { return TimeSpan.FromSeconds(TimeToStationInSeconds); }
    }
}