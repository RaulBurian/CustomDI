namespace CustomDI.DependencyInjection;

public class DiContainerScope : IDisposable
{
    private readonly DiContainer _diContainer;
    private readonly List<ServiceDescriptor> _descriptorsToDispose = [];

    public DiContainerScope(IEnumerable<ServiceDescriptor> services)
    {
        _diContainer = new DiContainer(services.Select(service =>
        {
            if (service.Lifetime == ServiceLifetime.Scoped)
            {
                _descriptorsToDispose.Add(service);
                
                return service with { Lifetime = ServiceLifetime.Singleton };
            }

            return service;
        }).ToList());
    }

    public TService GetService<TService>()
    {
        return _diContainer.GetService<TService>();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        foreach (var serviceDescriptor in _descriptorsToDispose)
        {
            if (serviceDescriptor is { Implementation: IDisposable disposable })
            {
                disposable.Dispose();
            }
        }
    }
}
