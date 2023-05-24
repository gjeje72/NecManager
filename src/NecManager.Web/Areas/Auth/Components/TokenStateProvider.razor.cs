namespace NecManager.Web.Areas.Auth.Components;

using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using NecManager.Web.Service.Identity;

/// <summary>
///     Custom auth token state provider, uses browser local storage.
/// </summary>
public partial class TokenStateProvider
{
    /// <summary>
    ///     Constante d'authentification de Token.
    /// </summary>
    private const string authToken = "DlService-Lcs-authToken";

    /// <summary>
    ///     Constante d'authentification de Token expiré.
    /// </summary>
    private const string authTokenExpires = "DlService-Lcs-authTokenExpires";

    /// <summary>
    ///     gets or set if it s loaded.
    /// </summary>
    private bool hasLoaded;

    /// <summary>
    ///     Gets or sets the child content. Which in this case represent all app.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <summary>
    ///     Gets or sets JSinterop wrapper to access locel browser storage.
    /// </summary>
    [Inject]
    public ProtectedLocalStorage LocalStorage { get; set; } = null!;

    /// <summary>
    ///     Gets or sets The web app authentication provider.
    /// </summary>
    [Inject]
    public ICustomAuthentificationStateProvider AuthProvider { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the web app context accessor.
    /// </summary>
    [Inject]
    public IHttpContextAccessor HttpContextAccessor { get; set; } = null!;

    /// <summary>
    ///     Gets or sets The app logger from DI.
    /// </summary>
    [Inject]
    public ILogger<TokenStateProvider> Logger { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the authentication token retrieved from localStorage.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    ///     Gets or sets the authentication token expiration date retrieved from local storage.
    /// </summary>
    public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     Stores token and expiration date in local storage.
    ///     When <seealso cref="Token" /> is null informations are removed from local storage.
    /// </summary>
    /// <returns>
    ///     result of the asynchronous operation.
    /// </returns>
    public async Task SaveChangesAsync()
    {
        try
        {
            if (this.Token == null)
            {
                await this.LocalStorage.DeleteAsync(authToken);
                await this.LocalStorage.DeleteAsync(authTokenExpires);
            }
            else
            {
                await this.LocalStorage.SetAsync("login", authToken, this.Token);
                await this.LocalStorage.SetAsync("login", authTokenExpires, this.ExpirationDate);
            }
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Error occured while saving data into local storage.");
        }
    }

    /// <summary>
    ///     Convert given unix millisec timestamp into Datetime.
    /// </summary>
    /// <param name="unixTimeStamp">timestamp to convert.</param>
    /// <returns>DateTime instance.</returns>
    public DateTime ToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
        return dtDateTime;
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        this.AuthProvider.AuthenticationStateChanged += this.OnAuthProviderOnAuthenticationStateChanged;

        try
        {
            var tokenRetriever = await this.LocalStorage.GetAsync<string>("login", authToken).ConfigureAwait(false);
            if (tokenRetriever.Success && tokenRetriever.Value != null)
            {
                this.Token = tokenRetriever.Value;
            }

            var expirationDateRetrieverTask = await this.LocalStorage.GetAsync<DateTime>("login", authTokenExpires).ConfigureAwait(false);
            if (expirationDateRetrieverTask.Success)
            {
                this.ExpirationDate = expirationDateRetrieverTask.Value;
            }
        }
        catch (Exception)
        {
            await this.SaveChangesAsync();
        }

        this.AuthProvider.SetToken(this.Token, this.ExpirationDate);
        this.hasLoaded = true;
    }

    private async void OnAuthProviderOnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        try
        {
            await this.AuthProvider_AuthenticationStateChanged(task);
        }
        catch (Exception e)
        {
            this.Logger.LogError(e, "An error occurs when token provide process.");
        }
    }

    /// <summary>
    ///     Each time authentication state changed,
    ///     we retrieve token and expiration date from user identity to update the local storage.
    /// </summary>
    /// <param name="task">The authentication state.</param>
    private async Task AuthProvider_AuthenticationStateChanged(Task<AuthenticationState> task)
    {
        var identity = task.Result;
        if (identity.User.Identity?.IsAuthenticated ?? false)
        {
            var expTimestamp = identity.User.Claims.FirstOrDefault(claim => claim.Type == "exp")?.Value;

            this.ExpirationDate = double.TryParse(expTimestamp, out var convertedExpTimestamp) ? this.ToDateTime(convertedExpTimestamp) : DateTime.UtcNow;

            this.Token = await this.AuthProvider.GetTokenAsync().ConfigureAwait(false);
        }
        else
        {
            this.Token = null;
        }

        // Store info in local storage
        await this.SaveChangesAsync().ConfigureAwait(false);
    }
}
