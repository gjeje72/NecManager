namespace NecManager_DAL.Model;

using Microsoft.AspNetCore.Identity;

/// <summary>
///     Defines an application user.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    ///     Gets or sets the firstname of the user.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    ///     Gets or sets the lastname of the user.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    ///     Gets or sets if a user is disabler.
    /// </summary>
    public bool? Disabled { get; set; }
}
