namespace NecManager.Web.Service.Models.AuthModule;

/// <summary>
///     Objects that contains user identification details necessary to authenticate a user.
/// </summary>
public sealed class UserLoginDetails
{
    /// <summary>
    ///     Gets or sets the email of the login.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets or set the Password of the login.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
