namespace NecManager.Web.Service.Models;
using NecManager.Common.DataEnum;
using System.Collections.Generic;

/// <summary>
///     Class which represents the information details for a group.
///     Used when group creating.
/// </summary>
public sealed class GroupDetails
{
    /// <summary>
    ///     Gets or sets a title for this group.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a collection of categories.
    /// </summary>
    public List<CategorieType>? Categories { get; set; }

    /// <summary>
    ///     Gets or sets a weapon.
    /// </summary>
    public WeaponType Weapon { get; set; }

    /// <summary>
    ///     Gets or sets a collection of students.
    /// </summary>
    public List<StudentBase>? Students { get; set; }
}
