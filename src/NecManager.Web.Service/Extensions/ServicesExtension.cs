namespace NecManager.Web.Service.Extensions;

using System;
using System.Net.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using NecManager.Web.Service.ApiServices;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Provider;

public static class ServicesExtension
{
    public static IServiceCollection AddServiceClient(this IServiceCollection services)
    {
        services.TryAddScoped<HttpClient>();
        services.TryAddTransient<RestHttpService>();
        services.AddBackendHttpClient(RestHttpService.StudentClientName, "students/");
        services.AddBackendHttpClient(RestHttpService.GroupClientName, "groups/");

        services.TryAddTransient<IStudentServices, StudentServices>();
        services.TryAddTransient<IGroupServices, GroupServices>();

        return services;
    }

    private static void AddBackendHttpClient(this IServiceCollection services, string clientName, string relativeUri)
        => services.AddHttpClient(clientName, (provider, httpClient) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var eatMapiUriBase = new Uri(configuration["Api:Url"]);

            httpClient.BaseAddress = new(eatMapiUriBase, relativeUri);
        });
}
