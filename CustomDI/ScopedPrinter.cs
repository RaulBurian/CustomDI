namespace CustomDI;

public class ScopedPrinter
{
    private readonly Guid _guid = Guid.NewGuid();
    private readonly IPrinterService _printerService;

    public ScopedPrinter(IPrinterService printerService)
    {
        _printerService = printerService;
    }

    public void PrintString()
    {
        _printerService.PrintString();
        Console.WriteLine($"Scoped Printer GUID: {_guid}");
    }
}