using System.Collections.Immutable;
using BusBoard.Logging;
using BusBoard.Models.Domain;

namespace BusBoard.Services;

public class BusArrivalService(PostcodeClient postcodeClient, TflClient tflClient, ILogger logger)
{
    public async Task<ImmutableList<StopPoint>> GetArrivalsForPostcodeAsync(string postcode)
    {
        logger.Info("🔃 Searching for Postcode Details...");
        Postcode postcodeCoordinates;
        try
        {
            postcodeCoordinates = await postcodeClient.GetPostcodeDetails(postcode);
        }
        catch (Exception ex)
        {
            throw new Exception("Enter a valid postcode", ex);
        }

        logger.Info("🔃 Searching for Nearby Bus Stops...");
        var stopPoints = await tflClient.GetNearbyBusStops(
            postcodeCoordinates.Longitude,
            postcodeCoordinates.Latitude,
            2);

        logger.Info("🔃 Loading Arrivals for Nearby Bus Stops...");
        await Task.WhenAll(stopPoints.Select(async stop =>
        {
            stop.Arrivals = await tflClient.GetArrivals(stop.Id, 5);
        }));

        return stopPoints;
    }
}