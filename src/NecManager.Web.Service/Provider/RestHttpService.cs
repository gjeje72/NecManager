namespace NecManager.Web.Service.Provider;
using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using NecManager.Web.Service.Extensions;

internal sealed class RestHttpService
{
    /// <summary>
    ///     Constant which define the student http client name.
    /// </summary>
    public const string StudentClientName = "Student";

    private readonly IHttpClientFactory factory;

    private readonly IConfiguration configuration;

    /* TODO a implémenter avec l'authentification */
    //private readonly ICustomAuthentificationStateProvider apiAuthenticationStateProvider;

    private string serviceFunctionalId = string.Empty;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RestHttpService" /> class.
    /// </summary>
    /// <param name="factory">The http client factory.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="authStateProvider">The authentication state provider.</param>
    public RestHttpService(IHttpClientFactory factory, IConfiguration configuration)
    {
        this.factory = factory;
        this.configuration = configuration;
    }

    public Task<HttpClient> StudentClient => this.CreateAuthenticatedClientAsync(StudentClientName);

    /// <summary>
    ///     Method which set the functional id.
    /// </summary>
    /// <param name="functionalId">The functional id.</param>
    public void SetServiceMonitoringFunctionalId(string functionalId)
        => this.serviceFunctionalId = functionalId;

    private async Task<HttpClient> CreateAuthenticatedClientAsync(string clientName)
    {
        var client = this.factory.CreateClient(clientName);

        var technicalId = new Guid(this.configuration.GetValue<string>("TechnicalId"));
        client.DefaultRequestHeaders.AddMonitoringIds(new(this.serviceFunctionalId, technicalId));

        return client;
        /* TODO a implémenter avec l'authentification */
        //return await this.WithAuthenticationHeaderIfAnyAsync(client);
    }

}
