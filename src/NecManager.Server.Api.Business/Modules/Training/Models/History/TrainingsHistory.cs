namespace NecManager.Server.Api.Business.Modules.Training.Models.History;
using System.Collections.Generic;

public sealed class TrainingsHistory
{
    public int? GroupId { get; set; }

    public string? GroupName { get; set; }

    public List<TrainingHistory> Trainings { get; set; } = new();

    public List<TrainingStudentBase> GroupStudents { get; set; } = new();
}
