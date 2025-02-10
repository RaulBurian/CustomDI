namespace CustomDI;

public interface IConsoleLoggerService
{
    void PrintString();
}

public class ConsoleLoggerService : IConsoleLoggerService
{
    private readonly IGuidProvider _guidProvider;

    public ConsoleLoggerService(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public void PrintString()
    {
        Console.WriteLine($"Hello World {_guidProvider.NewGuid()}!");
    }
}