namespace NecManager.Web.Service.Extensions;

using Microsoft.Extensions.DependencyInjection;
using NecManager.Web.Service.Provider;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.ApiServices;

public static class ServicesExtension
{
    public static IServiceCollection AddServiceClient(this IServiceCollection services)
    {
        services.TryAddScoped<HttpClient>();
        services.TryAddTransient<RestHttpService>();
        services.AddBackendHttpClient(RestHttpService.StudentClientName, "students/");

        services.TryAddTransient<IStudentServices, StudentServices>();


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
