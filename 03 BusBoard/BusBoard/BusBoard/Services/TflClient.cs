using System.Collections.Immutable;
using BusBoard.Models;

namespace BusBoard.Services;

public class TflClient(IApiService apiService)
{
    public async Task<ImmutableList<Arrival>> GetArrivals(string stopCode)
    {
        var arrivals = await apiService.GetAsync<ImmutableList<Arrival>>($"StopPoint/{stopCode}/Arrivals/");

        var nearestArrivals = arrivals.OrderBy(arrival => arrival.GetTimeToStation()).Take(5).ToImmutableList();

        return nearestArrivals;
    }
}