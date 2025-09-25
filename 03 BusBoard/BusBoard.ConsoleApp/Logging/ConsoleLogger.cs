using BusBoard.Logging;

namespace BusBoard.ConsoleApp.Logging;

public class ConsoleLogger : ILogger
{
    public void Info(string message = "")
    {
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌  {message}");
        Console.ResetColor();
    }
}