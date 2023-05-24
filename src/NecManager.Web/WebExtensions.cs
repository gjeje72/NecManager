namespace NecManager.Web;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Identity;

/// <summary>
///     Static class which define services for DI.
/// </summary>
internal static class WebExtensions
{
    /// <summary>
    ///     Method which configure service for DI in web project.
    /// </summary>
    /// <param name="services">The services collection to inject.</param>
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<HttpContextAccessor>();

        services.AddScoped<ICustomAuthentificationStateProvider, AuthenticationProvider>();
        services.AddScoped(provider => (AuthenticationStateProvider)provider.GetRequiredService<ICustomAuthentificationStateProvider>());

        services.AddServiceClient();

        services.AddAutoMapper(typeof(WebExtensions));
    }
}
