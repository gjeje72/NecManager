namespace NecManager.Server.Api.Business.Modules.Training;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NecManager.Common;
using NecManager.Common.DataEnum;
using NecManager.Common.DataEnum.Internal;
using NecManager.Server.Api.Business.Modules.Lesson.Models;
using NecManager.Server.Api.Business.Modules.Training.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

using static NecManager.Common.ApiResponseError;

internal class TrainingBusiness : ITrainingBusiness
{
    private readonly ILogger<ITrainingBusiness> logger;
    private readonly ITrainingAccessLayer trainingAccessLayer;
    private readonly ILessonAccessLayer lessonAccessLayer;

    public TrainingBusiness(ILogger<ITrainingBusiness> logger, ITrainingAccessLayer trainingAccessLayer, ILessonAccessLayer lessonAccessLayer)
    {
        this.logger = logger;
        this.trainingAccessLayer = trainingAccessLayer;
        this.lessonAccessLayer = lessonAccessLayer;
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
                Items = pageableResult.Items.Select(t => this.MapTrainingToTrainingBase(t)),
                TotalElements = pageableResult.TotalElements,
            };
            return new ApiResponse<PageableResult<TrainingBase>>(monitoringIds, new(pageableLessons));
        }
        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponse<TrainingBase>> GetTrainingByIdAsync(ServiceMonitoringDefinition monitoringIds, int trainingId)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingTraining = await this.trainingAccessLayer.GetSingleAsync(x => x.Id == trainingId, false, x => x.Include(t => t.Group!).Include(t => t.PersonTrainings!).Include(t => t.Lesson!)).ConfigureAwait(false);
        return matchingTraining is null
            ? (new(monitoringIds, new(ApiResponseResultState.NotFound, ApiResponseError.LessonApiErrors.LessonNotFound)))
            : (new(monitoringIds, new(this.MapTrainingToTrainingBase(matchingTraining))));
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> CreateTrainingAsync(ServiceMonitoringDefinition monitoringIds, TrainingCreationInput input)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        if (input.StartTime >= input.EndTime || input.Date < new DateTime(2023,1,1))
            return new(monitoringIds, new(ApiResponseResultState.BadRequest, TrainingApiErrors.TrainingBadRequest));

        var dbLesson = new Training
        {
            Date = input.Date,
            StartTime = input.StartTime,
            EndTime = input.EndTime,
            GroupId = input.GroupId,
            LessonId = input.LessonId,
            PersonTrainings = input.Students.Select(s => new PersonTraining { IsIndividual = input.IsIndividual, MasterName = input.MasterName, StudentId = s.Id }).ToList()
        };

        try
        {
            await this.trainingAccessLayer.AddAsync(dbLesson).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while creating training. Date = {date}", input.Date);
            return new(monitoringIds, new(TrainingApiErrors.TrainingCreationFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> DeleteTrainingAsync(ServiceMonitoringDefinition monitoringIds, int trainingId)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingTraining = await this.trainingAccessLayer.GetSingleAsync(x => x.Id == trainingId, true).ConfigureAwait(false);
        if (matchingTraining is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, TrainingApiErrors.TrainingNotFound));

        try
        {
            await this.trainingAccessLayer.RemoveAsync(matchingTraining).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while trying to delete training Id={trainingId}", trainingId);
            return new(monitoringIds, new(TrainingApiErrors.TrainingDeletionFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> AddStudentsInTrainingAsync(ServiceMonitoringDefinition monitoringIds, TrainingUpdateStudentInput input)
    {
        var matchingTraining = await this.trainingAccessLayer.GetSingleAsync(x => x.Id == input.TrainingId, true).ConfigureAwait(false);
        if (matchingTraining is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, TrainingApiErrors.TrainingNotFound));

        var masterName = matchingTraining.PersonTrainings.FirstOrDefault()?.MasterName ?? string.Empty;
        var isIndiv = matchingTraining.PersonTrainings.FirstOrDefault()?.IsIndividual ?? false;

        foreach(var studentToAdd in input.Students)
        {
            matchingTraining.PersonTrainings.Add(new PersonTraining
            {
                StudentId = studentToAdd.Id,
                TrainingId = matchingTraining.Id,
                MasterName = masterName,
                IsIndividual = isIndiv
            });
        }
        try
        {
            await this.trainingAccessLayer.UpdateAsync(matchingTraining).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while updating training trainingId={trainingId} with students", input.TrainingId);
            return new(monitoringIds, new(TrainingApiErrors.TrainingUpdateFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> UpdateTrainingLessonAsync(ServiceMonitoringDefinition monitoringIds, int trainingId, int lessonId)
    {
        // ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingTraining = await this.trainingAccessLayer.GetSingleAsync(x => x.Id == trainingId, true).ConfigureAwait(false);
        if (matchingTraining is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, TrainingApiErrors.TrainingNotFound));

        var matchingLesson = await this.lessonAccessLayer.GetSingleAsync(x => x.Id == lessonId, true).ConfigureAwait(false);
        if (matchingLesson is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, LessonApiErrors.LessonNotFound));

        try
        {
            matchingTraining.LessonId = lessonId;

            await this.trainingAccessLayer.UpdateAsync(matchingTraining).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while updating training trainingId={trainingId} with lessonId = {lessonId}", trainingId, lessonId);
            return new(monitoringIds, new(TrainingApiErrors.TrainingUpdateFailure));
        }

        return new(monitoringIds, new());
    }

    private TrainingBase MapTrainingToTrainingBase(Training matchingTraining)
        => new TrainingBase()
        {
            Id = matchingTraining.Id,
            Date = matchingTraining.Date,
            StartTime = matchingTraining.StartTime,
            EndTime = matchingTraining.EndTime,
            Categories = matchingTraining.Group?.Categories,
            Weapon = matchingTraining.Lesson?.Weapon ?? WeaponType.None,
            IsIndividual = matchingTraining.PersonTrainings.Count() == 1 && (matchingTraining.PersonTrainings.FirstOrDefault()?.IsIndividual ?? false),
            MasterName = matchingTraining.PersonTrainings?.FirstOrDefault()?.MasterName ?? string.Empty,
            Students = matchingTraining.PersonTrainings?.Select(pt => new TrainingStudentBase
            {
                Id = pt.StudentId,
                FirstName = pt.Student?.FirstName ?? string.Empty,
                Name = pt.Student?.Name ?? string.Empty,
                Category = pt.Student?.Category ?? CategoryType.None,
            }).ToList() ?? new(),
            Lesson = matchingTraining.Lesson != null
                            ? new()
                            {
                                Id = matchingTraining.LessonId,
                                Title = matchingTraining.Lesson.Title,
                                Weapon = matchingTraining.Lesson.Weapon,
                                Difficulty = matchingTraining.Lesson.Difficulty,
                                Description = matchingTraining.Lesson.Description,
                                Content = matchingTraining.Lesson.Content,
                            }
                            : new(),
        };
}
