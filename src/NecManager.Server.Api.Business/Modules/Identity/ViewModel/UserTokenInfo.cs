namespace NecManager.Server.Api.Business.Modules.Identity.ViewModel;
using System;

/// <summary>
///     The login out info.
/// </summary>
public sealed class UserTokenInfo
{
    /// <summary>
    ///     Gets or sets the token of the login model.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the expiration date of the login model.
    /// </summary>
    public DateTime ExpirationDate { get; set; }
}
