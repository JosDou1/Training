using System.Collections.Immutable;
using System.Text;
using BusBoard.Models;
using BusBoard.Services;
using RestSharp;

Console.OutputEncoding = Encoding.UTF8;

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("🚌 BusBoard 🚌");
Console.ResetColor();

string? stopCode;

do
{
    Console.Write("\nStop Code > ");
    stopCode = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(stopCode))
        Console.WriteLine("❌  Invalid Stop Code");
} while (string.IsNullOrWhiteSpace(stopCode));

Console.WriteLine($"\n🔃 Loading Arrivals for {stopCode}...");

var tflClient = new TflClient(new ApiService(new RestClientWrapper("https://api.tfl.gov.uk")));

ImmutableList<Arrival> arrivals = [];
try
{
    arrivals = await tflClient.GetArrivals(stopCode);
}
catch (Exception exception)
{
    Console.WriteLine("❌  Failed to Retrieve Arrivals");
    Environment.Exit(-1);
}

Console.WriteLine();
foreach (var arrival in arrivals)
{
    Console.WriteLine(
        $"[{DateTime.Now + arrival.GetTimeToStation():HH:mm}] {arrival.LineName} -> {arrival.DestinationName}");
}