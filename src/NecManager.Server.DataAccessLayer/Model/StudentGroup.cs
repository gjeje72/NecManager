namespace NecManager.Server.DataAccessLayer.Model;

using NecManager.Server.DataAccessLayer.Model.Abstraction;

public sealed class StudentGroup : ADataObject
{
    /// <summary>
    ///     Gets or sets the student id.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    ///     Gets or sets the navigation property for student.
    /// </summary>
    public Student? Student { get; set; }

    /// <summary>
    ///     Gets or sets student id.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    ///     Gets or sets the navigation property for group.
    /// </summary>
    public Group? Group { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the student is the responsive master.
    /// </summary>
    public bool IsResponsiveMaster { get; set; }
}
