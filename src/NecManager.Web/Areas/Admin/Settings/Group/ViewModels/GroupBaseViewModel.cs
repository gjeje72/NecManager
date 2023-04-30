namespace NecManager.Web.Areas.Admin.Settings.Group.ViewModels;

using NecManager.Common.DataEnum;
using NecManager.Web.Areas.Admin.Student.ViewModel;
using System.Collections.Generic;

public sealed class GroupBaseViewModel
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
