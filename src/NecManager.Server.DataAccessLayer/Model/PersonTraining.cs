namespace NecManager.Server.DataAccessLayer.Model;

using NecManager.Server.DataAccessLayer.Model.Abstraction;

public sealed class PersonTraining : ADataObject
{
    public int TrainingId { get; set; }

    public Training? Training { get; set; }

    public int StudentId { get; set; }

    public Student? Student { get; set; }

    public bool IsIndividual { get; set; }
}
