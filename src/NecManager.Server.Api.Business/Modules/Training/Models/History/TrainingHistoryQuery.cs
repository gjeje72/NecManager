namespace NecManager.Server.Api.Business.Modules.Training.Models.History;
using System.Reflection;

using Microsoft.AspNetCore.Http;

public sealed record TrainingHistoryQuery(int Id, bool IsStudent)
{
    /// <summary>
    ///     Method which bind query parameters to query object.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="parameter">The parameter info to bind.</param>
    /// <returns>Returns the query object <see cref="TrainingHistoryQuery" />.</returns>
    public static ValueTask<TrainingHistoryQuery?> BindAsync(HttpContext httpContext, ParameterInfo parameter)
    {
        _ = int.TryParse(httpContext.Request.Query["id"], out var id);
        _ = bool.TryParse(httpContext.Request.Query["isStudent"], out var isStudent);

        return ValueTask.FromResult<TrainingHistoryQuery?>(new(
            id, isStudent));
    }
}
