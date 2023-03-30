namespace NecManager.Server.Api.Business.Modules.Lesson.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using NecManager.Common;
using NecManager.Common.DataEnum;

/// <summary>
///     Record which represent a query to filter the store collection.
/// </summary>
/// <param name="PageSize">The page size.</param>
/// <param name="CurrentPage">The current page.</param>
public sealed record LessonQueryInput(int PageSize, int CurrentPage, DifficultyType? DifficultyType = null, WeaponType? WeaponType = null, int? GroupId = null) : PageableQuery(PageSize, CurrentPage)
{
    /// <summary>
    ///     Method which bind query parameters to query object.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="parameter">The parameter info to bind.</param>
    /// <returns>Returns the query object <see cref="LessonQueryInput" />.</returns>
    public static ValueTask<LessonQueryInput?> BindAsync(HttpContext httpContext, ParameterInfo parameter)
    {
        _ = int.TryParse(httpContext.Request.Query["currentPage"], out var page);
        _ = int.TryParse(httpContext.Request.Query["pageSize"], out var pageSize);
        _ = int.TryParse(httpContext.Request.Query["groupId"], out var groupId);
        DifficultyType? difficultyType = Enum.TryParse<DifficultyType>(httpContext.Request.Query["difficultyType"], out var parsedDifficultyType) ? parsedDifficultyType : null;
        WeaponType? weaponType = Enum.TryParse<WeaponType>(httpContext.Request.Query["weaponType"], out var parsedWeaponType) ? parsedWeaponType : null;


        return ValueTask.FromResult<LessonQueryInput?>(new(pageSize == 0 ? 10 : pageSize, page == 0 ? 1 : page, difficultyType, weaponType, groupId == 0 ? null : groupId));
    }
}
