using System.Text.Json.Serialization;

namespace BusBoard.Models;

public class Arrival
{
    [JsonPropertyName("lineName")]
    public required string LineName {
        get;
        set; 
    }
    
    [JsonPropertyName("stationName")]
    public required string StationName {
        get;
        set; 
    }

    [JsonPropertyName("platformName")]
    public required string PlatformName
    {
        get;
        set;
    }

    [JsonPropertyName("destinationName")]
    public required string DestinationName
    {
        get;
        set;
    }

    [JsonPropertyName("towards")]
    public required string Towards
    {
        get;
        set;
    }

    [JsonPropertyName("timeToStation")]
    public required int TimeToStationInSeconds
    {
        get;
        set;
    }

    [JsonIgnore]
    public TimeSpan TimeToStation
    {
        get
        {
            return TimeSpan.FromSeconds(TimeToStationInSeconds);
        }
    }
}