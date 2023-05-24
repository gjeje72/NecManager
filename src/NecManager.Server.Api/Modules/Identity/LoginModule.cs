namespace NecManager.Server.Api.Modules.Identity;

using Microsoft.AspNetCore.Mvc;

using NecManager.Server.Api.Business;
using NecManager.Server.Api.Business.Modules.Identity;
using NecManager.Server.Api.Business.Modules.Identity.ViewModel;
using NecManager.Server.Api.ResponseHelpers;
using NecManager.Server.Api.ResponseHelpers.Extensions;


/// <summary>
///     Class which defines the authentication module.
/// </summary>
public sealed class LoginModule : IModule
{
    /// <inheritdoc />
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        => services.AddIdentityBusiness();

    /// <inheritdoc />
    public void ConfigureModule(IServiceProvider serviceProvider)
    {
    }

    /// <inheritdoc />
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        _ = endpoints.MapPost("/auth/login",
                async (ApiRequestHeaders requestHeaders, [FromServices] IIdentityService identity, [FromBody] LoginIn login)
                    => Results.Extensions.ApiResponse(await identity.LoginAsync(requestHeaders, login)))
            .ProducesApiResponse<UserTokenInfo>()
            .WithName("Try to log")
            .WithTags("Authentication");

        _ = endpoints.MapPost("/auth/renew",
                async (ApiRequestHeaders requestHeaders, [FromServices] IIdentityService identity, [FromBody] UserTokenInfo login)
                    => Results.Extensions.ApiResponse(await identity.RefreshToken(requestHeaders, login)))
            .ProducesApiResponse<UserTokenInfo>()
            .WithName("Refresh the token")
            .WithTags("Authentication");

        return endpoints;
    }
}
