using System.Collections.Immutable;
using System.Text;
using BusBoard.ConsoleApp.Logging;
using BusBoard.Models.Domain;
using BusBoard.Services;

Console.OutputEncoding = Encoding.UTF8;

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("🚌 BusBoard 🚌");
Console.ResetColor();

var logger = new ConsoleLogger();

string? postcode;

do
{
    Console.Write("\nPostcode > ");
    postcode = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(postcode))
        logger.Error("Invalid Postcode");
} while (string.IsNullOrWhiteSpace(postcode));

var postcodeClient = new PostcodeClient(new ApiService(new RestClientWrapper("https://api.postcodes.io")));
var tflClient = new TflClient(new ApiService(new RestClientWrapper("https://api.tfl.gov.uk")));

var busArrivalService = new BusArrivalService(postcodeClient, tflClient, logger);

ImmutableList<StopPoint> stopPoints;
try
{
    stopPoints = await busArrivalService.GetArrivalsForPostcodeAsync(postcode!);
}
catch (Exception ex)
{
    logger.Error($"Failed to retrieve arrivals: {ex.Message}");
    return;
}

foreach (var stopPoint in stopPoints)
{
    logger.Info($"\n🚏 {stopPoint.Name}");
    foreach (var arrival in stopPoint.Arrivals)
    {
        logger.Info(
            $"   [{DateTime.Now + arrival.GetTimeToStation():HH:mm}] {arrival.LineName} -> {arrival.DestinationName}");
    }
}