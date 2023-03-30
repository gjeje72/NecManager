namespace NecManager.Server.Api.Business.Modules.Lesson;
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NecManager.Common;
using NecManager.Common.DataEnum.Internal;
using NecManager.Server.Api.Business.Modules.Lesson.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

using static NecManager.Common.ApiResponseError;

internal class LessonBusiness : ILessonBusiness
{
    private readonly ILogger<ILessonBusiness> logger;
    private readonly ILessonAccessLayer lessonAccessLayer;

    public LessonBusiness(ILogger<ILessonBusiness> logger, ILessonAccessLayer lessonAccessLayer)
    {
        this.logger = logger;
        this.lessonAccessLayer = lessonAccessLayer;
    }

    /// <inheritdoc />
    public async Task<ApiResponse<PageableResult<LessonBase>>> GetAllAsync(ServiceMonitoringDefinition monitoringIds, LessonQueryInput query)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var (pageSize, currentPage, difficultyType, weaponType, groupId) = query;
        var pageableResult = await this.lessonAccessLayer.GetPageableCollectionAsync(new(pageSize, currentPage, difficultyType, weaponType, groupId), false);
        if (pageableResult.Items is not null)
        {
            var pageableLessons = new PageableResult<LessonBase>
            {
                Items = pageableResult.Items.Select(l =>
                new LessonBase
                {
                    Id = l.Id,
                    Content = l.Content,
                    Description = l.Description,
                    Difficulty = l.Difficulty,
                    Title = l.Title,
                    Weapon = l.Weapon,
                }),
                TotalElements = pageableResult.TotalElements,
            };
            return new ApiResponse<PageableResult<LessonBase>>(monitoringIds, new(pageableLessons));
        }
        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponse<LessonBase>> GetLessonByIdAsync(ServiceMonitoringDefinition monitoringIds, int lessonId)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingLesson = await this.lessonAccessLayer.GetSingleAsync(x => x.Id == lessonId).ConfigureAwait(false);
        return matchingLesson is null
            ? (new(monitoringIds, new(ApiResponseResultState.NotFound, LessonApiErrors.LessonNotFound)))
            : (new(monitoringIds, new(new LessonBase()
            {
                Id = matchingLesson.Id,
                Title = matchingLesson.Title,
                Difficulty = matchingLesson.Difficulty,
                Weapon = matchingLesson.Weapon,
                Description = matchingLesson.Description,
                Content = matchingLesson.Content,
            })));
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> CreateLessonAsync(ServiceMonitoringDefinition monitoringIds, LessonCreationInput input)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        if (string.IsNullOrWhiteSpace(input.Title) || string.IsNullOrWhiteSpace(input.Description) || string.IsNullOrWhiteSpace(input.Content))
            return new(monitoringIds, new(ApiResponseResultState.BadRequest, LessonApiErrors.LessonBadRequest));

        var dbLesson = new Lesson
        {
            Title = input.Title,
            Weapon = input.Weapon,
            Difficulty = input.Difficulty,
            Description = input.Description,
            Content = input.Content,
        };

        try
        {
            await this.lessonAccessLayer.AddAsync(dbLesson).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while creating lesson named={lessonName}", input.Title);
            return new(monitoringIds, new(LessonApiErrors.LessonCreationFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> DeleteLessonAsync(ServiceMonitoringDefinition monitoringIds, int lessonId)
    {
        //ArgumentNullException.ThrowIfNull(monitoringIds);

        var matchingLesson = await this.lessonAccessLayer.GetSingleAsync(x => x.Id == lessonId, true).ConfigureAwait(false);
        if (matchingLesson is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, LessonApiErrors.LessonNotFound));

        try
        {
            await this.lessonAccessLayer.RemoveAsync(matchingLesson).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while trying to delete lesson Id={lessonId}", lessonId);
            return new(monitoringIds, new(LessonApiErrors.LessonDeletionFailure));
        }

        return new(monitoringIds, new());
    }

    /// <inheritdoc />
    public async Task<ApiResponseEmpty> UpdateLessonAsync(ServiceMonitoringDefinition monitoringIds, int lessonId, LessonUpdateInput input)
    {
       // ArgumentNullException.ThrowIfNull(monitoringIds);

        if (string.IsNullOrWhiteSpace(input.Title) || string.IsNullOrWhiteSpace(input.Description) || string.IsNullOrWhiteSpace(input.Content))
            return new(monitoringIds, new(ApiResponseResultState.BadRequest, LessonApiErrors.LessonBadRequest));

        var matchingLesson = await this.lessonAccessLayer.GetSingleAsync(x => x.Id == lessonId, true).ConfigureAwait(false);
        if (matchingLesson is null)
            return new(monitoringIds, new(ApiResponseResultState.NotFound, LessonApiErrors.LessonNotFound));

        try
        {
            matchingLesson.Title = input.Title;
            matchingLesson.Weapon = input.Weapon;
            matchingLesson.Content = input.Content;
            matchingLesson.Description = input.Description;
            matchingLesson.Difficulty = input.Difficulty;

            await this.lessonAccessLayer.UpdateAsync(matchingLesson).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error occured while updating lesson named={lessonName}", input.Title);
            return new(monitoringIds, new(LessonApiErrors.LessonUpdateFailure));
        }

        return new(monitoringIds, new());
    }
}
