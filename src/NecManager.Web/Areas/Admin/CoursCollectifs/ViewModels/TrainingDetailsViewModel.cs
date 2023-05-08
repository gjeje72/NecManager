namespace NecManager.Web.Areas.Admin.CoursCollectifs.ViewModels;

using NecManager.Common.DataEnum;
using System.Collections.Generic;
using System;

public class TrainingDetailsViewModel
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

    ///// <summary>
    /////     Gets or sets a collection of categories.
    ///// </summary>
    //public string? Categories { get; set; }

    public string? GroupName { get; set; }

    public string? LessonName { get; set; }

    /// <summary>
    ///     Gets or sets a weapon.
    /// </summary>
    public WeaponType Weapon { get; set; }

    public bool IsIndividual { get; set; }

    public string? MasterName { get; set; }

    public List<TrainingStudentViewModel> TrainingStudents { get; set; } = new();

    public List<TrainingStudentViewModel> GroupStudents { get; set; } = new();

    public TrainingLessonViewModel Lesson { get; set; } = new();
}
