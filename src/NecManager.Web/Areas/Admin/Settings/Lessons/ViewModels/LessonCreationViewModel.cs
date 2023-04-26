namespace NecManager.Web.Areas.Admin.Settings.Lessons.ViewModels;

using NecManager.Common.DataEnum;

public class LessonCreationViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public WeaponType Weapon { get; set; }

    public DifficultyType Difficulty { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}
