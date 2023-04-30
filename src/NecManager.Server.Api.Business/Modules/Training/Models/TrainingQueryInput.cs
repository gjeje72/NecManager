namespace NecManager.Server.Api.Business.Modules.Training.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using NecManager.Common;
using NecManager.Common.DataEnum;

/// <summary>
///     Record which represent a query to filter the training collection.
/// </summary>
/// <param name="PageSize">The page size.</param>
/// <param name="CurrentPage">The current page.</param>
public sealed record TrainingQueryInput(
    int PageSize,
    int CurrentPage,
    DifficultyType? DifficultyType = null,
    WeaponType? WeaponType = null,
    int? GroupId = null,
    DateTime? Date = null,
    int? Season = null,
    int? StudentId = null,
    string? Filter = null,
    bool OnlyIndividual = false,
    string? MasterName = null) : PageableQuery(PageSize, CurrentPage)
{
    /// <summary>
    ///     Method which bind query parameters to query object.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="parameter">The parameter info to bind.</param>
    /// <returns>Returns the query object <see cref="TrainingQueryInput" />.</returns>
    public static ValueTask<TrainingQueryInput?> BindAsync(HttpContext httpContext, ParameterInfo parameter)
    {
        _ = int.TryParse(httpContext.Request.Query["currentPage"], out var page);
        _ = int.TryParse(httpContext.Request.Query["pageSize"], out var pageSize);
        _ = int.TryParse(httpContext.Request.Query["groupId"], out var groupId);
        _ = int.TryParse(httpContext.Request.Query["season"], out var season);
        _ = int.TryParse(httpContext.Request.Query["studentId"], out var studentId);
        DifficultyType? difficultyType = Enum.TryParse<DifficultyType>(httpContext.Request.Query["difficultyType"], out var parsedDifficultyType) ? parsedDifficultyType : null;
        WeaponType? weaponType = Enum.TryParse<WeaponType>(httpContext.Request.Query["weaponType"], out var parsedWeaponType) ? parsedWeaponType : null;
        DateTime? date = DateTime.TryParse(httpContext.Request.Query["date"], out var parsedDate) ? parsedDate : null;
        bool onlyIndividual = bool.TryParse(httpContext.Request.Query["onlyIndividual"], out var parsedOnlyInd) ? parsedOnlyInd : false;
        string? masterName = httpContext.Request.Query["masterName"];
        string? filter = httpContext.Request.Query["filter"];

        return ValueTask.FromResult<TrainingQueryInput?>(new(
            pageSize == 0 ? 10 : pageSize,
            page == 0 ? 1 : page,
            difficultyType,
            weaponType,
            groupId == 0 ? null : groupId,
            date,
            season == 0 ? null : season,
            studentId == 0 ? null : studentId,
            filter,
            onlyIndividual,
            masterName));
    }
}
