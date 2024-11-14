public sealed class StartupFilter : IStartupFilter
{
    private readonly IServiceCollection _services;

    public StartupFilter(IServiceCollection services)
    {
        _services = services;
    }

    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        foreach (var service in _services)
        {
            Console.WriteLine($"Service: {service.ServiceType.FullName}, Lifetime: {service.Lifetime}, Implementation: {service.ImplementationType?.FullName}");
        }
        return next;
    }
}