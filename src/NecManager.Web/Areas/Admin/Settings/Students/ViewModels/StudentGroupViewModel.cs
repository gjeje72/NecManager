namespace NecManager.Web.Areas.Admin.Settings.Students.ViewModels;

using NecManager.Common.DataEnum;

public class StudentGroupViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public WeaponType Weapon { get; set; }
}
