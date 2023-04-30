namespace NecManager.Web.Areas.Admin.Settings.Group.ViewModels;

using NecManager.Common.DataEnum;

public sealed class GroupStudentViewModel
{
    public int Id { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public CategoryType Categorie { get; set; }

    public WeaponType Weapon { get; set; }
}
