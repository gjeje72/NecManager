namespace NecManager.Server.Api.Business.Modules.Training.Models;
using System.Collections.Generic;

public sealed class TrainingUpdateStudentInput
{
    public int TrainingId { get; set; }

    public List<TrainingStudentBase> Students { get; set; } = new();
}
