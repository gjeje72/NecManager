namespace NecManager.Server.DataAccessLayer.Model.Abstraction;
using System;

/// <summary>
/// Base abstract definition of an object model.
/// Every objects must have a technical id, a creation date and a modification date.
/// </summary>
public abstract class ADataObject
{
    /// <summary>
    /// Gets or sets the primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the date and time creation of the object.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last modification of the object.
    /// </summary>
    public DateTime LastModifiedDate { get; set; }
}
