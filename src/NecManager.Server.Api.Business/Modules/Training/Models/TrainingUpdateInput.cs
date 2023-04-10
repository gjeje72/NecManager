namespace NecManager.Server.Api.Business.Modules.Training.Models;
public sealed class TrainingUpdateInput
{
    public int TrainingId { get; set; }

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
}
