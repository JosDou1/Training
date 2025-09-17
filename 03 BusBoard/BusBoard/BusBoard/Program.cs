using System.Collections.Immutable;
using System.Text;
using BusBoard.Models;
using BusBoard.Models.Domain;
using BusBoard.Services;

Console.OutputEncoding = Encoding.UTF8;

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("🚌 BusBoard 🚌");
Console.ResetColor();

string? postcode;

do
{
    Console.Write("\nPostcode > ");
    postcode = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(postcode))
        Console.WriteLine("❌  Invalid Postcode");
} while (string.IsNullOrWhiteSpace(postcode));

Console.WriteLine($"\n🔃 Searching for Postcode Details...");

var postcodeClient = new PostcodeClient(new ApiService(new RestClientWrapper("https://api.postcodes.io")));

Postcode postcodeCoordinates;
try
{
    postcodeCoordinates = await postcodeClient.GetPostcodeDetails(postcode);
}
catch (Exception exception)
{
    Console.WriteLine($"❌  Failed to Retrieve Postcode Details: {exception.Message}");
    Environment.Exit(-1);
    throw;
}

Console.WriteLine($"\n🔃 Searching for Nearby Bus Stops...");

var tflClient = new TflClient(new ApiService(new RestClientWrapper("https://api.tfl.gov.uk")));

ImmutableList<StopPoint> stopPoints = [];
try
{
    stopPoints = await tflClient.GetNearbyBusStops(postcodeCoordinates.Longitude, postcodeCoordinates.Latitude, 2);
}
catch (Exception exception)
{
    Console.WriteLine($"❌  Failed to Retrieve Nearby Bus Stops: {exception.Message}");
    Environment.Exit(-1);
}

Console.WriteLine($"\n🔃 Loading Arrivals for Nearby Bus Stops...");

try
{
    await Task.WhenAll(stopPoints
        .Select(async stopPoint => { stopPoint.Arrivals = await tflClient.GetArrivals(stopPoint.Id, 5); }));
}
catch (Exception exception)
{
    Console.WriteLine($"❌  Failed to Retrieve Arrivals: {exception.Message}");
    Environment.Exit(-1);
}

Console.WriteLine();

foreach (var stopPoint in stopPoints)
{
    Console.WriteLine($"\n🚏 {stopPoint.Name}");
    foreach (var arrival in stopPoint.Arrivals)
    {
        Console.WriteLine(
            $"   [{DateTime.Now + arrival.GetTimeToStation():HH:mm}] {arrival.LineName} -> {arrival.DestinationName}");
    }
}