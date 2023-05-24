namespace NecManager.Web.Service.Provider;
using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Identity;

internal sealed class RestHttpService
{
    /// <summary>
    ///     Constant which define the authentication http client name.
    /// </summary>
    public const string AuthClientName = "Auth";

    /// <summary>
    ///     Constant which define the student http client name.
    /// </summary>
    public const string StudentClientName = "Student";

    /// <summary>
    ///     Constant which define the group http client name.
    /// </summary>
    public const string GroupClientName = "Group";

    /// <summary>
    ///     Constant which define the lesson http client name.
    /// </summary>
    public const string LessonClientName = "Lesson";

    /// <summary>
    ///     Constant which define the training http client name.
    /// </summary>
    public const string TrainingClientName = "Trainings";

    private readonly IHttpClientFactory factory;

    private readonly IConfiguration configuration;

    private readonly ICustomAuthentificationStateProvider apiAuthenticationStateProvider;

    private string serviceFunctionalId = string.Empty;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RestHttpService" /> class.
    /// </summary>
    /// <param name="factory">The http client factory.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="authStateProvider">The authentication state provider.</param>
    public RestHttpService(IHttpClientFactory factory, IConfiguration configuration, ICustomAuthentificationStateProvider authStateProvider)
    {
        this.factory = factory;
        this.configuration = configuration;
        this.apiAuthenticationStateProvider = authStateProvider;
    }

    public Task<HttpClient> StudentClient => this.CreateAuthenticatedClientAsync(StudentClientName);

    public Task<HttpClient> GroupClient => this.CreateAuthenticatedClientAsync(GroupClientName);

    public Task<HttpClient> LessonClient => this.CreateAuthenticatedClientAsync(LessonClientName);

    public Task<HttpClient> TrainingClient => this.CreateAuthenticatedClientAsync(TrainingClientName);

    /// <summary>
    ///     Gets an user http client for auth calls.
    /// </summary>
    public Task<HttpClient> AuthClient => this.CreateAuthenticatedClientAsync(AuthClientName);

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

        return await this.WithAuthenticationHeaderIfAnyAsync(client);
    }

    /// <summary>
    ///     Adds a default authentication header with token from <seealso cref="LcsAuthenticationProvider" />.
    /// </summary>
    /// <remarks>If token is not available, the authorization header is set to null.</remarks>
    private async Task<HttpClient> WithAuthenticationHeaderIfAnyAsync(HttpClient client)
    {
        var token = await this.apiAuthenticationStateProvider.GetTokenAsync();
        client.DefaultRequestHeaders.Authorization = !string.IsNullOrWhiteSpace(token)
                                                         ? new("Bearer", token)
                                                         : null;

        return client;
    }
}
