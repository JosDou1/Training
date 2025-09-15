using BusBoard.Models;
using RestSharp;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var tflClient = new RestClient("https://api.tfl.gov.uk");

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("🚌 BusBoard 🚌");
Console.ResetColor();

string? stopCode;

do
{
    Console.Write("\nStop Code > ");
    stopCode = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(stopCode)) Console.WriteLine("❌  Invalid Stop Code");
} while (string.IsNullOrWhiteSpace(stopCode));

Console.WriteLine($"\n🔃 Loading Arrivals for {stopCode}...");

var request = new RestRequest($"StopPoint/{stopCode}/Arrivals/");
var response = await tflClient.ExecuteAsync<List<Arrival>>(request);

if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Data == null)
{
    Console.WriteLine($"❌  Failed to Retrieve Arrivals for {stopCode}");
    Environment.Exit(-1);
}

List<Arrival> arrivals = response.Data;

var nearestArrivals = arrivals.OrderBy(arrival => arrival.TimeToStation).Take(5).ToList();

Console.WriteLine();
foreach (var arrival in nearestArrivals)
{
    Console.WriteLine($"[{DateTime.Now + arrival.TimeToStation:HH:mm}] {arrival.LineName} -> {arrival.DestinationName}");
}

