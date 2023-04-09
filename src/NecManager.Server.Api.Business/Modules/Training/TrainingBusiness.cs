namespace NecManager.Server.Api.Business.Modules.Training;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

using NecManager.Common;
using NecManager.Common.DataEnum;
using NecManager.Server.Api.Business.Modules.Lesson.Models;
using NecManager.Server.Api.Business.Modules.Training.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;

internal class TrainingBusiness : ITrainingBusiness
{
    private readonly ILogger<ITrainingBusiness> logger;
    private readonly ITrainingAccessLayer trainingAccessLayer;

    public TrainingBusiness(ILogger<ITrainingBusiness> logger, ITrainingAccessLayer trainingAccessLayer)
    {
        this.logger = logger;
        this.trainingAccessLayer = trainingAccessLayer;
    }

    /// <inheritdoc />
    public async Task<ApiResponse<PageableResult<TrainingBase>>> SearchAsync(ServiceMonitoringDefinition monitoringIds, TrainingQueryInput query)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var (pageSize, currentPage, difficultyType, weaponType, groupId, date, season, studentId, onlyIndividual, masterName) = query;
        var pageableResult = await this.trainingAccessLayer.GetPageableCollectionAsync(new(pageSize, currentPage, difficultyType, weaponType, groupId, date, season, studentId, onlyIndividual, masterName), false);
        if (pageableResult.Items is not null)
        {
            var pageableLessons = new PageableResult<TrainingBase>
            {
                Items = pageableResult.Items.Select(t =>
                new TrainingBase
                {
                    Id = t.Id,
                    Date = t.Date,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Categories = t.Group?.Categories,
                    Weapon = t.Lesson?.Weapon ?? WeaponType.None,
                    IsIndividual = t.PersonTrainings.Count() == 1 && (t.PersonTrainings.FirstOrDefault()?.IsIndividual ?? false),
                    MasterName = t.PersonTrainings?.FirstOrDefault()?.MasterName ?? string.Empty,
                    Students = t.PersonTrainings?.Select(pt => new TrainingStudentBase
                    {
                        Id = pt.StudentId,
                        FirstName = pt.Student?.FirstName ?? string.Empty,
                        Name = pt.Student?.Name ?? string.Empty,
                        Category = pt.Student?.Category ?? CategoryType.None,
                    }).ToList() ?? new(),
                    Lesson = t.Lesson != null
                    ? new()
                    {
                        Id = t.LessonId,
                        Title = t.Lesson.Title,
                        Weapon = t.Lesson.Weapon,
                        Difficulty = t.Lesson.Difficulty,
                        Description = t.Lesson.Description,
                        Content = t.Lesson.Content,
                    }
                    : new(),
                }),
                TotalElements = pageableResult.TotalElements,
            };
            return new ApiResponse<PageableResult<TrainingBase>>(monitoringIds, new(pageableLessons));
        }
        return new(monitoringIds, new());
    }
}
