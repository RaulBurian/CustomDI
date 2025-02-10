namespace CustomDI.DependencyInjection;

public class DiContainer
{
    private readonly List<ServiceDescriptor> _services;

    public DiContainer(List<ServiceDescriptor> services)
    {
        _services = services;
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

        if ((descriptor.Implementation, descriptor.Lifetime) is (not null, ServiceLifetime.Singleton))
        {
            return descriptor.Implementation;
        }

        if (descriptor.ImplementationFactory is not null)
        {
            var service = descriptor.ImplementationFactory(this);

            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                _services[idx] = descriptor with { Implementation = service };
            }
            
            return service;
        }

        var actualServiceType = descriptor.ImplementationType ?? descriptor.ServiceType;

        if (actualServiceType.IsAbstract || actualServiceType.IsInterface)
        {
            throw new NotRegisteredException($"No service which can be instantiated for the {actualServiceType.FullName} is registered");
        }

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
}
