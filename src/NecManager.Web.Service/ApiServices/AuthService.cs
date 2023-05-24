namespace NecManager.Web.Service.ApiServices;

using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using NecManager.Common;
using NecManager.Web.Service.ApiServices.Abstractions;
using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Models.AuthModule;

using NecManager.Web.Service.Provider;

/// <summary>
///     Class which represents the service for the authentication.
/// </summary>
internal sealed class AuthService : ServiceBase, IAuthService
{
    private readonly HttpContext httpContext;
    /// <summary>
    ///     Initializes a new instance of the <see cref="AuthService" /> class.
    /// </summary>
    /// <param name="restHttpService">The http rest service provider.</param>
    public AuthService(RestHttpService restHttpService, IHttpContextAccessor httpContextAccessor)
        : base(restHttpService, "AUTH_S001")
    {
        this.httpContext = httpContextAccessor.HttpContext;
    }

    /// <summary>
    ///     Service to Login the user.
    /// </summary>
    /// <param name="login">The login.</param>
    /// <returns>Returns the token.</returns>
    public async Task<ServiceResult<TokenDto>> LoginAsync(UserLoginDetails login)
    {
        using var content = login.ToStringContent();
        var requestUri = new Uri("login", UriKind.Relative);

        var authClient = await this.RestHttpService.AuthClient;

        // Used to log remote ip for connection audit
        var ip = this.httpContext.Connection.RemoteIpAddress;
        if (ip is not null)
            authClient.DefaultRequestHeaders.Add("REMOTE_ADDR", ip.ToString());

        var response = await authClient.PostAsync(requestUri, content).ConfigureAwait(false);

        return await response.BuildDataServiceResultAsync<TokenDto>().ConfigureAwait(false);
    }
}
