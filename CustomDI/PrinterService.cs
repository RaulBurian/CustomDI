namespace CustomDI;

public interface IPrinterService
{
    void PrintString();
}

public class FirstPrinterService : IPrinterService
{
    private readonly int _randomNr = Random.Shared.Next();
    private readonly IGuidProvider _guidProvider;

    public FirstPrinterService(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public void PrintString()
    {
        Console.WriteLine($"First string {_guidProvider.NewGuid()} and {_randomNr}");
    }
}
