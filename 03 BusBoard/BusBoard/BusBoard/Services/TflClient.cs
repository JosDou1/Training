using System.Collections.Immutable;
using BusBoard.Models;

namespace BusBoard.Services;

public class TflClient
{
    private readonly IApiService _apiService;

    public TflClient(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<ImmutableList<Arrival>> GetArrivals(string stopCode)
    {
        var arrivals = await _apiService.GetAsync<ImmutableList<Arrival>>($"StopPoint/{stopCode}/Arrivals/");

        var nearestArrivals = arrivals.OrderBy(arrival => arrival.GetTimeToStation).Take(5).ToImmutableList();

        return nearestArrivals;
    }
}