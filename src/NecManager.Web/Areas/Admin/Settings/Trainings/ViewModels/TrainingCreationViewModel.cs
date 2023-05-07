namespace NecManager.Web.Areas.Admin.Settings.Trainings.ViewModels;

using System.Collections.Generic;
using System;

public class TrainingCreationViewModel
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    /// <summary>
    ///     The start time for this training. 16h30 = 16,50
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    ///     The end time for this training. 16h45 = 16,75
    /// </summary>
    public DateTime EndTime { get; set; }

    public int LessonId { get; set; }

    public int? GroupId { get; set; }

    public List<TrainingStudentViewModel> Students { get; set; } = new();

    public bool IsIndividual { get; set; }

    public string? MasterName { get; set; }

    public string StudentFilter { get; set; } = string.Empty;

    public int? StudentId { get; set; }
}
