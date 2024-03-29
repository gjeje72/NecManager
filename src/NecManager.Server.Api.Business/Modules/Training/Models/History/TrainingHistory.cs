﻿namespace NecManager.Server.Api.Business.Modules.Training.Models.History;

public sealed class TrainingHistory
{
    public DateTime Date { get; set; }

    public bool IsIndividual { get; set; }

    public List<string> TrainingStudentsIds { get; set; } = new();

}
