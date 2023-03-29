namespace NecManager.Web.Service.Models;

using NecManager.Common.DataEnum;

using System.Collections.Generic;

/// <summary>
///     Class which represents the minimal information for a group.
/// </summary>
public class GroupBase
{
    /// <summary>
    ///     Gets or sets the id for this group.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets a title for this group.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a weapon linked to this group.
    /// </summary>
    public WeaponType Weapon { get; set; }

    /// <summary>
    ///     Gets or sets a collection of categories.
    /// </summary>
    public List<CategoryType>? Categories { get; set; }

    /// <summary>
    ///     Gets or sets a collection of categories ids linked to this group.
    /// </summary>
    public List<int>? CategoriesIds { get; set; }

    /// <summary>
    ///     Gets or sets a number indicating how many students are register to this group.
    /// </summary>
    public int StudentCount { get; set; }
}
