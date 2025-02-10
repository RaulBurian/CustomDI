namespace CustomDI.DependencyInjection;

public class DiContainer
{
    private readonly List<ServiceDescriptor> _services;

    public DiContainer(List<ServiceDescriptor> services)
    {
        _services = services;
    }

    public DiContainerScope BeginScope()
    {
        return new DiContainerScope(_services);
    }

    public TService GetService<TService>()
    {
        return (TService)GetService(typeof(TService));
    }

    private object GetService(Type type)
    {
        var idx = _services.FindIndex(serviceDescriptor => serviceDescriptor.ServiceType == type);
        var descriptor = idx > -1 ? _services[idx] : null;

        if (descriptor is null)
        {
            throw new NotRegisteredException($"Service of type {type.FullName} is not registered");
        }

        if (descriptor is { Implementation: not null, Lifetime: ServiceLifetime.Singleton })
        {
            return ImplementationSingleton(descriptor);
        }

        if (descriptor.ImplementationFactory is not null)
        {
            return ImplementationFactory(descriptor, idx);
        }

        var actualServiceType = descriptor.ImplementationType ?? descriptor.ServiceType;

        if (actualServiceType.IsAbstract || actualServiceType.IsInterface)
        {
            throw new NotRegisteredException($"No service which can be instantiated for the {actualServiceType.FullName} is registered");
        }

        var implementation = ResolveImplementation(actualServiceType, descriptor, idx);

        return implementation;
    }

    private object ResolveImplementation(Type actualServiceType, ServiceDescriptor descriptor, int idx)
    {
        var primaryConstructorInfo = actualServiceType.GetConstructors().First();
        var constructorParameters = primaryConstructorInfo.GetParameters()
            .Select(parameter => GetService(parameter.ParameterType))
            .ToArray();

        var implementation = Activator.CreateInstance(actualServiceType, constructorParameters)!;

        if (descriptor.Lifetime is ServiceLifetime.Singleton)
        {
            _services[idx] = descriptor with { Implementation = implementation };
        }

        return implementation;
    }

    private static object ImplementationSingleton(ServiceDescriptor descriptor)
    {
        return descriptor.Implementation!;
    }

    private object ImplementationFactory(ServiceDescriptor descriptor, int idx)
    {
        var service = descriptor.ImplementationFactory!(this);

        if (descriptor.Lifetime == ServiceLifetime.Singleton)
        {
            _services[idx] = descriptor with { Implementation = service };
        }
            
        return service;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        foreach (var serviceDescriptor in _services)
        {
            if (serviceDescriptor is { Lifetime: ServiceLifetime.Singleton, Implementation: IDisposable disposable })
            {
                disposable.Dispose();
            }
        }
    }
}
