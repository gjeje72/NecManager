namespace NecManager.Server.Api.Business.Modules.Student.Models;

using System.Reflection;

using Microsoft.AspNetCore.Http;

using NecManager.Common;
using NecManager.Common.DataEnum;

public sealed record StudentQueryInput(int PageSize, int CurrentPage, WeaponType? WeaponType = null, int? GroupId = null, StudentState? State = null, string? Filter = null, bool? isPageable = true) : PageableQuery(PageSize, CurrentPage)
{
    /// <summary>
    ///     Method which bind query parameters to query object.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="parameter">The parameter info to bind.</param>
    /// <returns>Returns the query object <see cref="StudentQueryInput" />.</returns>
    public static ValueTask<StudentQueryInput?> BindAsync(HttpContext httpContext, ParameterInfo parameter)
    {
        _ = int.TryParse(httpContext.Request.Query["currentPage"], out var page);
        _ = int.TryParse(httpContext.Request.Query["pageSize"], out var pageSize);
        _ = int.TryParse(httpContext.Request.Query["groupId"], out var groupId);
        StudentState? studentState = Enum.TryParse<StudentState>(httpContext.Request.Query["studentState"], out var parsedDifficultyType) ? parsedDifficultyType : null;
        WeaponType? weaponType = Enum.TryParse<WeaponType>(httpContext.Request.Query["weaponType"], out var parsedWeaponType) ? parsedWeaponType : null;
        string? filter = httpContext.Request.Query["filter"];
        _ = bool.TryParse(httpContext.Request.Query["isPageable"], out var isPageable);


        return ValueTask.FromResult<StudentQueryInput?>(new(pageSize == 0 ? 10 : pageSize, page == 0 ? 1 : page, weaponType, groupId == 0 ? null : groupId, studentState, filter, isPageable));
    }
}
