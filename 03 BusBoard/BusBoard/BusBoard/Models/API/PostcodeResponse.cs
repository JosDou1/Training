using System.Text.Json.Serialization;

namespace BusBoard.Models.API;

public class PostcodeResponse
{
    [JsonPropertyName("result")] public required ResultData Result { get; set; }

    public class ResultData
    {
        [JsonPropertyName("longitude")] public float Longitude { get; set; }

        [JsonPropertyName("latitude")] public float Latitude { get; set; }
    }
}