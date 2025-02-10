namespace CustomDI.DependencyInjection;

public record ServiceDescriptor
{
    public ServiceDescriptor(Type serviceType, ServiceLifetime serviceLifetime)
    {
        ServiceType = serviceType;
        Lifetime = serviceLifetime;
    }

    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = serviceLifetime;
    }
    
    public ServiceDescriptor(Type serviceType, Func<DiContainer, object> implementationFactory, ServiceLifetime serviceLifetime)
    {
        ServiceType = serviceType;
        ImplementationFactory = implementationFactory;
        Lifetime = serviceLifetime;
    }

    public ServiceDescriptor(Type serviceType, object implementation, ServiceLifetime serviceLifetime)
    {
        ServiceType = serviceType;
        ImplementationType = implementation.GetType();
        Implementation = implementation;
        Lifetime = serviceLifetime;
    }

    public Type ServiceType { get; }

    public Type? ImplementationType { get; }

    public object? Implementation { get; init; }

    public Func<DiContainer, object>? ImplementationFactory { get; }

    public ServiceLifetime Lifetime { get; }
}

public enum ServiceLifetime
{
    Singleton,
    Transient,
    Scoped,
}