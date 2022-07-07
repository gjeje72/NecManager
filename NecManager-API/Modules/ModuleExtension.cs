namespace NecManager_API.Modules;

/// <summary>
///     Provides extension methods to work with <seealso cref="IModule"/>.
/// </summary>
public static class ModuleExtension
{
    /// <summary>
    ///     Registers all modules of this project that implements <seealso cref="IModule"/>.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>Returns the service collection</returns>
    public static IServiceCollection RegisterModules(this IServiceCollection services)
    {
        var modules = DiscoverModules();

        foreach (var module in modules)
        {
            services.AddTransient(typeof(IModule), module);
        }

        services.RegisterModulesDependencies();

        return services;
    }

    public static IApplicationBuilder UseModuleBuildActions(this IApplicationBuilder app)
    {
        var modules = app.ApplicationServices.GetServices<IModule>();
        foreach (var module in modules)
        {
            module.ConfigureModule(app.ApplicationServices);
        }

        return app;
    }

    /// <summary>
    ///     Registers all modules dependencies.
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <returns>Returns the service collection.</returns>
    private static IServiceCollection RegisterModulesDependencies(this IServiceCollection services)
    {
        var modules = GetRegisteredModules(services);
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        foreach (var module in modules)
        {
            module.RegisterModule(services, configuration);
        }

        return services;
    }

    /// <summary>
    ///     Adds route endpoints for all modules.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <returns>The endpoint route builder</returns>
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints, IServiceCollection services)
    {
        var modules = GetRegisteredModules(services);

        foreach (var module in modules)
        {
            module.MapEndpoints(endpoints);
        }

        return endpoints;
    }

    /// <summary>
    ///     Returns the collection of modules declared in this project.
    /// </summary>
    /// <returns>
    ///     Returns a module collection.
    /// </returns>
    private static IEnumerable<Type> DiscoverModules()
        => typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)));

    /// <summary>
    ///     Returns all modules instances.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>Return a collection of <seealso cref="IModule"/> instances</returns>
    private static IEnumerable<IModule> GetRegisteredModules(IServiceCollection services)
        => services.BuildServiceProvider().GetServices<IModule>();
}