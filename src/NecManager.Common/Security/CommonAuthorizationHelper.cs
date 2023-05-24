namespace NecManager.Common.Security;

using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

using NecManager.Common.DataEnum.Identity;

/// <summary>
///     Classe which contains authorization helping methods.
/// </summary>
public static class CommonAuthorizationHelper
{
    /// <summary>
    ///     Method which add admin policy.
    /// </summary>
    /// <param name="options">The authorization options.</param>
    public static void PoliciesOptions(AuthorizationOptions options)
    {
        options.AddPolicy(PolicyName.IsAdmin, policy => policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, RoleType.ADMIN.ToString()));

        options.AddPolicy(PolicyName.IsUser, policy => policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, RoleType.ADMIN.ToString(), RoleType.USER.ToString()));
    }
}
