namespace NecManager.Server.Api.Security;

using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

using NecManager.Common.DataEnum.Identity;
using NecManager.Common.Security;

public class UserAuthorizationContext
{
    private readonly IAuthorizationService authorizationService;
    private readonly HttpContext? httpContext;

    public UserAuthorizationContext(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
    {
        this.authorizationService = authorizationService;
        this.httpContext = httpContextAccessor.HttpContext;
    }

    /// <summary>
    ///     Gets the calling user.
    /// </summary>
    public ClaimsPrincipal? User => this.httpContext?.User;

    /// <summary>
    ///     Indicates wheter the user is Admin.
    /// </summary>
    /// <returns>true if user is admin, false otherwise.</returns>
    public bool IsAdmin()
        => this.httpContext?.User.IsInRole(RoleType.ADMIN.ToString()) ?? false;

    /// <summary>
    ///     Gets the current scoped user email address.
    /// </summary>
    /// <returns>The email address of the current user.</returns>
    public string? GetUserEmailAddress()
    {
        var userEmailAddress = this.httpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimName.UserEmailAddress)?.Value;

        return !string.IsNullOrEmpty(userEmailAddress)
            ? userEmailAddress
            : null;
    }
}
