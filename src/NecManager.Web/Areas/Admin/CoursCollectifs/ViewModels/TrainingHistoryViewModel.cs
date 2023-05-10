namespace NecManager.Web.Areas.Admin.CoursCollectifs.ViewModels;

using System.Collections.Generic;
using System;

public class TrainingHistoryViewModel
{
    public DateTime Date { get; set; }

    public bool IsIndividual { get; set; }

    public List<int> TrainingStudentsIds { get; set; } = new();
}
