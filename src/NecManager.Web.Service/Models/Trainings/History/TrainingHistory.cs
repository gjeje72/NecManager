namespace NecManager.Web.Service.Models.Trainings.History;

using System;
using System.Collections.Generic;

public sealed class TrainingHistory
{
    public DateTime Date { get; set; }

    public bool IsIndividual { get; set; }

    public List<string> TrainingStudentsIds { get; set; } = new();

}
