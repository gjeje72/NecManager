namespace NecManager.Web.Areas.Admin.Settings.Trainings.ViewModels;

using System;
using System.Collections.Generic;

using NecManager.Web.Service.Models.Trainings;

public class SeasonTrainingCreation
{
    public List<TrainingCreationInput> Trainings { get; set; } = new();

    public string MasterName { get; set; } = string.Empty;

    public int GroupId { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime EndDate { get; set; } = DateTime.Now;

    public DateTime HolidayStartDate { get; set; } = DateTime.Now;

    public DateTime HolidayEndDate { get; set; } = DateTime.Now;

    public DateTime PublicHolidayDate { get; set; } = DateTime.Now;

    public string HolidayErrorMessage { get; set; } = string.Empty;

    public List<DateTime> ExcludedDates { get; set; } = new();

    public bool isMondaySelected { get; set; }

    public DateTime MondayStartTime { get; set; }

    public DateTime MondayEndTime { get; set; }

    public bool isTuesdaySelected { get; set; }

    public DateTime TuesdayStartTime { get; set; }

    public DateTime TuesdayEndTime { get; set; }

    public bool isWednesdaySelected { get; set; }

    public DateTime WednesdayStartTime { get; set; }

    public DateTime WednesdayEndTime { get; set; }

    public bool isThursdaySelected { get; set; }

    public DateTime ThursdayStartTime { get; set; }

    public DateTime ThursdayEndTime { get; set; }

    public bool isFridaySelected { get; set; }

    public DateTime FridayStartTime { get; set; }

    public DateTime FridayEndTime { get; set; }

    public bool isSaturdaySelected { get; set; }

    public DateTime SaturdayStartTime { get; set; }

    public DateTime SaturdayEndTime { get; set; }

    public bool isSundaySelected { get; set; }

    public DateTime SundayStartTime { get; set; }

    public DateTime SundayEndTime { get; set; }
}
