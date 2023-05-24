namespace NecManager.Server.Api.Business.Modules.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using NecManager.Common;
using NecManager.Common.DataEnum.Internal;
using NecManager.Common.Security;
using NecManager.Server.Api.Business.Modules.Identity.ViewModel;

using NecManager.Server.DataAccessLayer.Model;

/// <summary>
///     Sample business service to retrieve identity data.
/// </summary>
internal sealed class IdentityService : IIdentityService
{
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<Student> userManager;
    private readonly IConfiguration configuration;
    private readonly ILogger<IdentityService> log;

    /// <summary>
    ///     Initializes a new instance of the <see cref="IdentityService" /> class.
    /// </summary>
    /// <param name="roleManager">The role manager.</param>
    /// <param name="userManager">The user manager.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="log">The logger for current class.</param>
    public IdentityService(RoleManager<IdentityRole> roleManager, UserManager<Student> userManager, IConfiguration configuration, ILogger<IdentityService> log)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        this.configuration = configuration;
        this.log = log;
    }

    /// <inheritdoc />
    public ApiResponse<IEnumerable<string>> GetRoles(ServiceMonitoringDefinition monitoringIds)
    {
        var roles = this.roleManager.Roles.ToList().Select(role => role.NormalizedName);

        return new(monitoringIds, new(roles));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<UserTokenInfo>> LoginAsync(ServiceMonitoringDefinition monitoringIds, LoginIn login)
    {
        var user = await this.userManager.FindByNameAsync(login.UserName).ConfigureAwait(false);

        if (user is null)
        {
            user = await this.userManager.FindByEmailAsync(login.UserName).ConfigureAwait(false);

            if (user is null)
            {
                return new(monitoringIds, new(ApiResponseResultState.BadRequest, ApiResponseError.Auth.AuthenticationFailed));
            }
        }

        if (await this.userManager.IsLockedOutAsync(user))
        {
            return new(monitoringIds, new(ApiResponseError.Auth.LoginUserBlocked));
        }

        var authResult = await this.userManager.CheckPasswordAsync(user, login.Password).ConfigureAwait(false);

        if (!authResult)
        {
            return new(monitoringIds, new(ApiResponseError.Auth.AuthenticationFailed));
        }

        var userClaims = await this.BuildClaimsAsync(user).ConfigureAwait(false);

        var loginOut = this.GetToken(userClaims);

        return new(monitoringIds, new(loginOut));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<UserTokenInfo>> RefreshToken(ServiceMonitoringDefinition monitoringIds, UserTokenInfo? login)
    {
        if (login == null || string.IsNullOrWhiteSpace(login.Token))
        {
            return new(monitoringIds, new(ApiResponseError.RefreshTokenError));
        }

        var oldToken = new JwtSecurityTokenHandler().ReadJwtToken(login.Token);

        // Search the user name is sub claim
        var claimSub = oldToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub);
        if (claimSub == null)
            return new(monitoringIds, new(ApiResponseError.RefreshTokenError));

        var user = await this.userManager.FindByNameAsync(claimSub.Value).ConfigureAwait(false);
        if (user == null)
            return new(monitoringIds, new(ApiResponseError.RefreshTokenError));

        var newToken = this.GetToken(oldToken.Claims);

        return new(monitoringIds, new(newToken));
    }

    /// <summary>
    ///     Method to build the claims.
    /// </summary>
    /// <param name="user">The connected user.</param>
    /// <returns>Returns the user claims.</returns>
    private async Task<IEnumerable<Claim>> BuildClaimsAsync(Student user) => new[]
       {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(ClaimName.UserFirstName, user.FirstName?.ToString() ?? string.Empty),
            new Claim(ClaimName.UserLastName, user.Name?.ToString() ?? string.Empty),
            new Claim(ClaimName.UserEmailAddress, user.Email ?? string.Empty),
        }
       .Union(await this.userManager.GetClaimsAsync(user))
       .Union((await this.userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

    /// <summary>
    ///     Method to get the token.
    /// </summary>
    /// <param name="userClaims">The user claims.</param>
    /// <returns>Returns the login out info.</returns>
    private UserTokenInfo GetToken(IEnumerable<Claim> userClaims)
    {
        // This two lines of code define the signing key and algorithm which being use as the token credentials
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:SecurityKey"]));
        var tokenCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expirationDate = DateTime.UtcNow.AddSeconds(int.Parse(this.configuration.GetValue<string>("Jwt:ExpiryInSeconds"), CultureInfo.InvariantCulture));

        var token = new JwtSecurityToken(
            issuer: this.configuration.GetValue<string>("Jwt:Issuer"),
            audience: this.configuration.GetValue<string>("Jwt:Audience"),
            claims: userClaims,
            expires: expirationDate,
            signingCredentials: tokenCredentials);

        var loginOut = new UserTokenInfo
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpirationDate = expirationDate,
        };
        return loginOut;
    }
}
