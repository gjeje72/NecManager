namespace NecManager.Server.DataAccessLayer.Model;

using NecManager.Server.DataAccessLayer.Model.Abstraction;

public sealed class Team : ADataObject
{
    public ICollection<Student> TeamMates { get; set; } = [];

    public string Name { get; set; } = string.Empty;
}
