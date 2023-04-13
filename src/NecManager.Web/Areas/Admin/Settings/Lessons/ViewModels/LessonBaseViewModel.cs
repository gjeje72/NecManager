namespace NecManager.Web.Areas.Admin.Settings.Lessons.ViewModels;

using NecManager.Common.DataEnum;

public class LessonBaseViewModel
{
    public string Title { get; set; } = string.Empty;

    public WeaponType Weapon { get; set; }

    public DifficultyType DifficultyType { get; set; }
}
