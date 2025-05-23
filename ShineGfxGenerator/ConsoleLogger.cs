namespace ShineGfxGenerator;

/// <summary>
/// Basic console logging with color support
/// </summary>
public static class ConsoleLogger
{
    public static void WriteLine(string? message, ConsoleColor color = ConsoleColor.White)
    {
        WriteColoredLine(message, color);
    }
    
    public static void WriteErrorLine(string? message)
    {
        WriteColoredLine(message, ConsoleColor.Red);
    }

    private static void WriteColoredLine(string? message, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Error.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }
}