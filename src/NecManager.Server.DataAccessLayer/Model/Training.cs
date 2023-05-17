namespace NecManager.Server.DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using NecManager.Server.DataAccessLayer.Model.Abstraction;

public sealed class Training : ADataObject
{
    public DateTime Date { get; set; }

    /// <summary>
    ///     The start time for this training. 16h30 = 16,50
    /// </summary>
    public decimal StartTime { get; set; }

    /// <summary>
    ///     The end time for this training. 16h45 = 16,75
    /// </summary>
    public decimal EndTime { get; set; }

    [MaxLength(200)]
    public string? MasterName { get; set; }

    public ICollection<PersonTraining> PersonTrainings { get; set; } = new HashSet<PersonTraining>();

    public int LessonId { get; set; }

    public Lesson? Lesson { get; set; }

    public int? GroupId { get; set; } = null;

    public Group? Group { get; set; }
}
