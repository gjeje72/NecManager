namespace NecManager.Web.Service.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

using NecManager.Common.DataEnum.Internal;
using NecManager.Common.Security;
using NecManager.Web.Service.Extensions;
using NecManager.Web.Service.Models.AuthModule;
using NecManager.Web.Service.Provider;

public sealed class AuthenticationProvider : AuthenticationStateProvider, ICustomAuthentificationStateProvider
{
    private readonly ILogger<AuthenticationProvider> logger;

    /// <summary>
    ///     Local instance access of the current HttpContext (set from DI).
    /// </summary>
    private readonly HttpClient client;
    private string? Token { get; set; }

    private DateTime TokenExpiration { get; set; }

    private ClaimsPrincipal? Principal { get; set; }

    private IAuthorizationService authorizationService;

    public AuthenticationProvider(ILogger<AuthenticationProvider> logger, IHttpClientFactory factory, IAuthorizationService authorizationService)
    {
        this.logger = logger;
        this.client = factory.CreateClient(RestHttpService.AuthClientName);
        this.authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public string UserFullname => $"{this.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimName.UserFirstName)?.Value} {this.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimName.UserLastName)?.Value}";

    /// <inheritdoc />
    public string Login => $"{this.Principal?.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value}";

    /// <inheritdoc />
    public string Email => $"{this.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimName.UserEmailAddress)?.Value}";

    /// <inheritdoc />
    public async Task<string?> GetTokenAsync()
    {
        var token = this.Token;
        if (token == null)
            return string.Empty;

        var tokenExpiration = this.TokenExpiration;
        if (DateTime.UtcNow >= tokenExpiration)
        {
            var result = await this.UpdateTokenAsync().ConfigureAwait(false);
            this.SetToken(result.token, result.expiration);
        }

        return this.Token;
    }

    /// <inheritdoc />
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await this.GetTokenAsync().ConfigureAwait(false);
        var identity = string.IsNullOrEmpty(token)
                           ? new()
                           : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        this.Principal = new(identity);

        return new(this.Principal ?? new());
    }

    /// <summary>
    ///     Method which is used to set a user as authenticate.
    /// </summary>
    /// <param name="token">JWT.</param>
    /// <param name="expiration">expiration datetime of the jwt.</param>
    public void SetUserLoggedIn(string? token, DateTime expiration)
    {
        this.SetToken(token, expiration);
    }

    /// <summary>
    ///     Remove token value, to be use when user needs / wants to sign off.
    /// </summary>
    /// <param name="token">JWT.</param>
    /// <param name="expiration">expiration datetime of the jwt.</param>
    public void LogOutUser()
    {
        this.SetToken(null, DateTime.UtcNow);
    }

    /// <summary>
    ///     Update token value and expiration datetime.
    /// </summary>
    /// <param name="token">JWT.</param>
    /// <param name="expiration">expiration datetime of the jwt.</param>
    public void SetToken(string? token, DateTime expiration)
    {
        this.Token = token;
        this.TokenExpiration = expiration;

        this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
    }

    /// <summary>
    ///     Read JWT to extract claims values.
    /// </summary>
    /// <param name="jwt">JWT as string.</param>
    /// <returns>Collection of claims extracted from JWT.</returns>
    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        if (string.IsNullOrEmpty(jwt))
        {
            return new List<Claim>();
        }

        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);

        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs == null)
        {
            return new List<Claim>();
        }

        keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

        if (roles != null)
        {
            if (roles.ToString()?.Trim().StartsWith("[", StringComparison.CurrentCultureIgnoreCase) ?? false)
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!) ?? Array.Empty<string>();

                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new(ClaimTypes.Role, roles.ToString() ?? string.Empty));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)));

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }

    /// <summary>
    ///     Method that tries to renew current JWT from identity server.
    /// </summary>
    /// <returns>tuple with token and expiration date.</returns>
    private async Task<(string? token, DateTime expiration)> UpdateTokenAsync()
    {
        var token = this.Token;
        var tokenRenewObject = new TokenDto
        {
            Token = token
        };

        var identityResult = await this.client.PostAsync(new Uri("renew", UriKind.Relative), tokenRenewObject.ToStringContent()).ConfigureAwait(false);

        if (identityResult is not { IsSuccessStatusCode: true })
        {
            return (null, DateTime.UtcNow);
        }

        var (state, tokenDto, errorMessage) = await identityResult.BuildDataServiceResultAsync<TokenDto>();
        if (state != ServiceResultState.Success || tokenDto is null)
        {
            this.logger.LogError(errorMessage.FirstOrDefault());
            return (null, DateTime.UtcNow);
        }

        return (tokenDto.Token, tokenDto.ExpirationDate);
    }
}
