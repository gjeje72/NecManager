namespace NecManager.Web.Areas.Admin.Student.ViewModel;

using System.Collections.Generic;

using NecManager.Common.DataEnum;
using NecManager.Web.Areas.Admin.Settings.Group.ViewModels;

public sealed class CreateGroupViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public List<CategoryType> Categories { get; set; } = new();

    public WeaponType Weapon { get; set; }

    public List<GroupStudentViewModel> Students { get; set; } = new();
}
