using CustomDI;
using CustomDI.DependencyInjection;

var services = new DiServiceCollection();

services.RegisterSingleton<RandomNumberService>();
services.RegisterSingleton<IPrinterService, FirstPrinterService>();
services.RegisterSingleton<IGuidProvider>(new GuidProvider());
services.RegisterSingleton<IConsoleLoggerService>(sp =>
{
    var guidService = sp.GetService<IGuidProvider>();
    return new ConsoleLoggerService(guidService);
});
services.RegisterScoped(new ScopedPrinter());
    
var diContainer = services.BuildServiceProvider();

var randomService1 = diContainer.GetService<RandomNumberService>();
var randomService2 = diContainer.GetService<RandomNumberService>();

Console.WriteLine(randomService1.GetRandom());
Console.WriteLine(randomService2.GetRandom());
Console.WriteLine("--------------------------------");

var printService1 = diContainer.GetService<IPrinterService>();
var printService2 = diContainer.GetService<IPrinterService>();

printService1.PrintString();
printService2.PrintString();
Console.WriteLine("--------------------------------");


var consoleLogger1 = diContainer.GetService<IConsoleLoggerService>();
var consoleLogger2 = diContainer.GetService<IConsoleLoggerService>();

consoleLogger1.PrintString();
consoleLogger2.PrintString();
Console.WriteLine("--------------------------------");


using (var scope = diContainer.BeginScope())
{
    var scopedPrinter1 = scope.GetService<ScopedPrinter>();
    var scopedPrinter2 = scope.GetService<ScopedPrinter>();
    scopedPrinter1.PrintString();
    scopedPrinter2.PrintString();
    Console.WriteLine("--------------------------------");
    
    var guidProvider = scope.GetService<IGuidProvider>();
    Console.WriteLine(guidProvider.NewGuid());
}

var guidProvider1 = diContainer.GetService<IGuidProvider>();
Console.WriteLine(guidProvider1.NewGuid());