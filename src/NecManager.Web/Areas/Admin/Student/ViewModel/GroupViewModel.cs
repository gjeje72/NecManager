namespace NecManager.Web.Areas.Admin.Student.ViewModel;

using System.Collections.Generic;

using NecManager.Common.DataEnum;

public class GroupViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public List<CategoryType> Categories { get; set; } = new();

    public WeaponType Weapon { get; set; }

    /// <summary>
    ///     Gets or sets a number indicating how many students are register to this group.
    /// </summary>
    public int StudentCount { get; set; }

    public List<StudentViewModel>? Students { get; set; } = new();
}
