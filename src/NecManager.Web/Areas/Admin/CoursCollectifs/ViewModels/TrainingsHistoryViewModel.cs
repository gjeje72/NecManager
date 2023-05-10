namespace NecManager.Web.Areas.Admin.CoursCollectifs.ViewModels;

using System.Collections.Generic;

public class TrainingsHistoryViewModel
{
    public int? GroupId { get; set; }

    public string? GroupName { get; set; }

    public List<TrainingHistoryViewModel> Trainings { get; set; } = new();

    public List<TrainingStudentViewModel> GroupStudents { get; set; } = new();
}
