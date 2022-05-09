namespace DataEnum.Identity;

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
    ///     DL SERVICES employee.
    /// </summary>
    [Display(ResourceType = typeof(RoleResource), Description = "EMPLOYEE")]
    EMPLOYEE = 1,

    /// <summary>
    ///     User.
    /// </summary>
    [Display(ResourceType = typeof(RoleResource), Description = "USER")]
    USER = 2,

    /// <summary>
    ///     Manager.
    /// </summary>
    [Display(ResourceType = typeof(RoleResource), Description = "MAN")]
    MANAGER = 3,
}
