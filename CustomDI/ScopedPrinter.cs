namespace CustomDI;

public class ScopedPrinter
{
    private readonly Guid _guid = Guid.NewGuid();
    
    public void PrintString()
    {
        Console.WriteLine($"Scoped Printer {_guid}");
    }
}