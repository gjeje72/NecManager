namespace NecManager.Web.Service.Models;

/// <summary>
///     Class which represents the minimal information for a group.
/// </summary>
public sealed class GroupBase
{
    /// <summary>
    ///     Gets or sets the id for this group.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets a title for this group.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}
