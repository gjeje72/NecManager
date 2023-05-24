namespace NecManager.Server.Api.Business.Modules.Training.Models.History;
using System.Reflection;

using Microsoft.AspNetCore.Http;

public sealed record TrainingHistoryQuery(int Id, string StudentId)
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
        var studentId = httpContext.Request.Query["studentId"];

        return ValueTask.FromResult<TrainingHistoryQuery?>(new(
            id, studentId));
    }
}
