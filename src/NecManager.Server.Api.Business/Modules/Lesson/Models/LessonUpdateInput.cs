namespace NecManager.Server.Api.Business.Modules.Lesson.Models;

using NecManager.Common.DataEnum;

public sealed class LessonUpdateInput
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public WeaponType Weapon { get; set; }

    public DifficultyType Difficulty { get; set; }

    public string Description { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}
