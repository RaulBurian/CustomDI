namespace CustomDI.DependencyInjection;

public class DiServiceCollection
{
    private readonly List<ServiceDescriptor> _services = [];

    public DiServiceCollection RegisterSingleton<TService>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), ServiceLifetime.Singleton));

        return this;
    }

    public DiServiceCollection RegisterSingleton<TInterface, TService>() where TService : TInterface where TInterface : class
    {
        _services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TService), ServiceLifetime.Singleton));

        return this;
    }

    public DiServiceCollection RegisterSingleton<TService>(TService implementation) where TService : class
    {
        _services.Add(new ServiceDescriptor(typeof(TService), implementation, ServiceLifetime.Singleton));

        return this;
    }

    public DiServiceCollection RegisterSingleton<TService>(Func<DiContainer, TService> factory) where TService : class
    {
        _services.Add(new ServiceDescriptor(typeof(TService), (Func<DiContainer, object>)FactoryDelegate, ServiceLifetime.Singleton));

        return this;

        object FactoryDelegate(DiContainer sp) => factory(sp);
    }
    
    public DiServiceCollection RegisterScoped<TService>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), ServiceLifetime.Scoped));

        return this;
    }

    public DiServiceCollection RegisterScoped<TInterface, TService>() where TService : TInterface where TInterface : class
    {
        _services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TService), ServiceLifetime.Scoped));

        return this;
    }

    public DiServiceCollection RegisterScoped<TService>(Func<DiContainer, TService> factory) where TService : class
    {
        _services.Add(new ServiceDescriptor(typeof(TService), (Func<DiContainer, object>)FactoryDelegate, ServiceLifetime.Scoped));

        return this;

        object FactoryDelegate(DiContainer sp) => factory(sp);
    }

    public DiServiceCollection RegisterTransient<TService>()
    {
        _services.Add(new ServiceDescriptor(typeof(TService), ServiceLifetime.Transient));

        return this;
    }

    public DiServiceCollection RegisterTransient<TInterface, TService>() where TService : TInterface where TInterface : class
    {
        _services.Add(new ServiceDescriptor(typeof(TInterface), typeof(TService), ServiceLifetime.Transient));

        return this;
    }

    public DiServiceCollection RegisterTransient<TService>(Func<DiContainer, TService> factory) where TService : class
    {
        _services.Add(new ServiceDescriptor(typeof(TService), (Func<DiContainer, object>)FactoryDelegate, ServiceLifetime.Transient));

        return this;

        object FactoryDelegate(DiContainer sp) => factory(sp);
    }

    public DiContainer BuildServiceProvider()
    {
        return new DiContainer(_services);
    }
}