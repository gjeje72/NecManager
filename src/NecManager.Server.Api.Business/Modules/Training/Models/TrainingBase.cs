namespace NecManager.Server.Api.Business.Modules.Training.Models;
using NecManager.Common.DataEnum;

public sealed class TrainingBase
{
    public int Id { get; set; }

    public int? GroupId { get; set; }

    public int LessonId { get; set; }

    public DateTime Date { get; set; }

    /// <summary>
    ///     The start time for this training. 16h30 = 16,50
    /// </summary>
    public decimal StartTime { get; set; }

    /// <summary>
    ///     The end time for this training. 16h45 = 16,75
    /// </summary>
    public decimal EndTime { get; set; }

    public string? GroupName { get; set; }

    public string? LessonName { get; set; }

    /// <summary>
    ///     Gets or sets a weapon.
    /// </summary>
    public WeaponType Weapon { get; set; }

    public bool IsIndividual { get; set; }

    public string? MasterName { get; set; }
}
