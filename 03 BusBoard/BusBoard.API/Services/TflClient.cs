using System.Collections.Immutable;
using BusBoard.Models;
using BusBoard.Models.API;
using BusBoard.Models.Domain;
using RestSharp;

namespace BusBoard.Services;

public class TflClient(IApiService apiService)
{
    private readonly ImmutableList<string> busStopTypes =
    [
        "NaptanPublicBusCoachTram"
    ];

    public async Task<ImmutableList<Arrival>> GetArrivals(string stopCode, int numberOfArrivals)
    {
        var arrivals = await apiService.GetAsync<ImmutableList<Arrival>>($"StopPoint/{stopCode}/Arrivals/");

        var nearestArrivals = arrivals.OrderBy(arrival => arrival.GetTimeToStation()).Take(numberOfArrivals)
            .ToImmutableList();

        return nearestArrivals;
    }

    public async Task<ImmutableList<StopPoint>> GetNearbyBusStops(float longitude, float latitude, int numberOfStops)
    {
        var stopTypesString = string.Join(",", busStopTypes);

        var request = new RestRequest("StopPoint")
            .AddQueryParameter("stopTypes", stopTypesString)
            .AddQueryParameter("lon", longitude)
            .AddQueryParameter("lat", latitude);

        var nearbyStops =
            await apiService.GetAsync<NearbyStopsResponse>(request);

        var closestStops =
            nearbyStops.StopPoints.OrderBy(stop => stop.Distance).Take(numberOfStops).ToImmutableList();

        return closestStops;
    }
}