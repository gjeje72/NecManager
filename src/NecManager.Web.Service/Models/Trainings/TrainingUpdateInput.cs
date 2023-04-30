namespace NecManager.Web.Service.Models.Trainings;
using System;
using System.Collections.Generic;

public sealed class TrainingUpdateInput
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    /// <summary>
    ///     The start time for this training. 16h30 = 16,50
    /// </summary>
    public decimal StartTime { get; set; }

    /// <summary>
    ///     The end time for this training. 16h45 = 16,75
    /// </summary>
    public decimal EndTime { get; set; }

    public int LessonId { get; set; }

    public int? GroupId { get; set; }

    public List<TrainingStudentBase> Students { get; set; } = new();

    public bool IsIndividual { get; set; }

    public string? MasterName { get; set; }
}
