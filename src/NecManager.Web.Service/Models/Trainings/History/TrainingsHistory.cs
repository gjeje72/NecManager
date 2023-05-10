namespace NecManager.Web.Service.Models.Trainings.History;
using System.Collections.Generic;

using NecManager.Web.Service.Models;

public sealed class TrainingsHistory
{
    public int? GroupId { get; set; }

    public string? GroupName { get; set; }

    public List<TrainingHistory> Trainings { get; set; } = new();

    public List<TrainingStudentBase> GroupStudents { get; set; } = new();
}
