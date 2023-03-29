namespace NecManager.Server.Api.Business.Modules.Group.Models;
using NecManager.Common.DataEnum;

public class GroupOutputBase
{
    /// <summary>
    ///     Gets or sets the Id for this group.
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
    ///     Gets or sets a collection of categories ids linked to this group.
    /// </summary>
    public List<int>? CategoriesIds { get; set; }

    /// <summary>
    ///     Gets or sets a number indicating how many students are register to this group.
    /// </summary>
    public int StudentCount { get; set; }
}
