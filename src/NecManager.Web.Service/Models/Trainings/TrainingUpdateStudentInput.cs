namespace NecManager.Web.Service.Models.Trainings;
using System.Collections.Generic;

public sealed class TrainingUpdateStudentInput
{
    public int TrainingId { get; set; }

    public List<string> StudentsIds { get; set; } = new();

    public string? MasterName { get; set; }

    public bool IsIndividual { get; set; }
}
