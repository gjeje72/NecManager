namespace NecManager.Web.Service.Identity;
using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Authorization;

/// <summary>
///     Custom immplementation of the global AuthenticationProvider.
/// </summary>
public interface ICustomAuthentificationStateProvider
{
    /// <summary>
    ///     An event that provides notification when the Microsoft.AspNetCore.Components.Authorization.AuthenticationState
    ///     has changed. For example, this event may be raised if a user logs in or out.
    /// </summary>
    public event AuthenticationStateChangedHandler AuthenticationStateChanged;

    /// <summary>
    ///     Gets the authentified user firstname and lastname.
    /// </summary>
    string UserFullname { get; }

    /// <summary>
    ///     Gets the authentified user login.
    /// </summary>
    string Login { get; }

    /// <summary>
    ///     Gets the authentified user email.
    /// </summary>
    string Email { get; }

    /// <summary>
    ///     Get current JWT token, check if its stil valid and trigger an update if needed.
    /// </summary>
    /// <returns>jwt as string.</returns>
    Task<string?> GetTokenAsync();

    /// <summary>
    ///     Method to set the token.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="expiration">The expiration date and time of the authentication token.</param>
    void SetToken(string? token, DateTime expiration);

    /// <summary>
    ///     Method to logout the user.
    /// </summary>
    void LogOutUser();

    /// <summary>
    ///     Method to set the user logged in.
    /// </summary>
    /// <param name="token">The authentication token.</param>
    /// <param name="expiration">The expiration date and time of the authentication token.</param>
    void SetUserLoggedIn(string? token, DateTime expiration);
}
