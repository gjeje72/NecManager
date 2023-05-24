namespace NecManager.Server.Api.Business.Modules.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Server.Api.Business.Modules.Identity.ViewModel;

/// <summary>
///     Defines the exposed business methods.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    ///     Retrieves roles.
    /// </summary>
    /// <param name="monitoringIds">Moniroting ids to help tracing issues.</param>
    /// <returns>Collection of user roles if any.</returns>
    ApiResponse<IEnumerable<string>> GetRoles(ServiceMonitoringDefinition monitoringIds);

    /// <summary>
    ///     Authenticate a user.
    /// </summary>
    /// <param name="monitoringIds">Moniroting ids to help tracing issues.</param>
    /// <param name="login">User identification parameters.</param>
    /// <returns>The authentication token details for user.</returns>
    Task<ApiResponse<UserTokenInfo>> LoginAsync(ServiceMonitoringDefinition monitoringIds, LoginIn login);

    /// <summary>
    ///     Refresh a token.
    /// </summary>
    /// <param name="monitoringIds">Moniroting ids to help tracing issues.</param>
    /// <param name="login">User refresh token info.</param>
    /// <returns>The authentication token details for user.</returns>
    Task<ApiResponse<UserTokenInfo>> RefreshToken(ServiceMonitoringDefinition monitoringIds, UserTokenInfo login);
}
