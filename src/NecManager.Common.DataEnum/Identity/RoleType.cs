namespace NecManager.Common.DataEnum.Identity;
using System.ComponentModel.DataAnnotations;

/// <summary>
///     All available roles in this application.
/// </summary>
public enum RoleType
{
    /// <summary>
    ///     Admin.
    /// </summary>
    [Display(ResourceType = typeof(RoleResource), Description = "ADMIN")]
    ADMIN = 0,

    /// <summary>
    ///     User.
    /// </summary>
    [Display(ResourceType = typeof(RoleResource), Description = "USER")]
    USER = 1,
}

