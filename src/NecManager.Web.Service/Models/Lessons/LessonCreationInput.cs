namespace NecManager.Web.Service.Models.Lessons;

using NecManager.Common.DataEnum;

public sealed class LessonCreationInput
{
    public string Title { get; set; } = string.Empty;

    public WeaponType Weapon { get; set; }

    public DifficultyType Difficulty { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}
