namespace NecManager.Server.DataAccessLayer.Model;

using System.ComponentModel.DataAnnotations;

using NecManager.Common.DataEnum;
using NecManager.Server.DataAccessLayer.Model.Abstraction;

public class Lesson : ADataObject
{
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public WeaponType Weapon { get; set; }

    public DifficultyType Difficulty { get; set; }

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;

    public virtual ICollection<Training> Trainings { get; set; } = new List<Training>();
}
