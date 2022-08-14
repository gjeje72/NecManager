namespace NecManager.Server.Api.Modules;

using System.Reflection;

/// <summary>
///     Provides extension methods to work with <seealso cref="IModule" />.
/// </summary>
public static class ModuleExtension
{
    /// <summary>
    ///     Registers all modules of this project that implements <seealso cref="IModule" />.
    /// </summary>
    /// <typeparam name="TAssembly">The type from current assembly.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>Returns the service collection.</returns>
    public static IServiceCollection RegisterModules<TAssembly>(this IServiceCollection services)
    {
        var modules = DiscoverModules(typeof(TAssembly).Assembly);

        foreach (var module in modules)
        {
            services.AddTransient(typeof(IModule), module);
        }

        services.RegisterModulesDependencies();

        return services;
    }

    public static IApplicationBuilder UseModulesBuildActions(this IApplicationBuilder app)
    {
        var modules = app.ApplicationServices.GetServices<IModule>();
        foreach (var module in modules)
        {
            module.ConfigureModule(app.ApplicationServices);
        }

        return app;
    }

    /// <summary>
    ///     Adds route endpoints for all modules.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>Returns the endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapModulesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var modules = GetRegisteredModules(endpoints.ServiceProvider);

        foreach (var module in modules)
        {
            module.MapEndpoints(endpoints);
        }

        return endpoints;
    }

    /// <summary>
    ///     Registers all modules dependencies.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <returns>Returns the service collection.</returns>
    private static IServiceCollection RegisterModulesDependencies(this IServiceCollection services)
    {
        var modules = GetRegisteredModules(services.BuildServiceProvider());
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        foreach (var module in modules)
        {
            module.RegisterModule(services, configuration);
        }

        return services;
    }

    /// <summary>
    ///     Returns the collection of modules declared in this project.
    /// </summary>
    /// <param name="assembly">The assembly to discover.</param>
    /// <returns>
    ///     Returns a module collection.
    /// </returns>
    private static IEnumerable<Type> DiscoverModules(Assembly assembly)
        => assembly
          .GetTypes()
          .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)));

    /// <summary>
    ///     Returns all modules instances.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>Return a collection of <seealso cref="IModule" /> instances</returns>
    private static IEnumerable<IModule> GetRegisteredModules(IServiceProvider serviceProvider)
        => serviceProvider.GetServices<IModule>();
}
