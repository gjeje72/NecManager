namespace NecManager.Server.Api.ResponseHelpers;

using System.Net;

using NecManager.Common;
using NecManager.Common.DataEnum.Internal;


/// <summary>
///     Class which represent an api response result.
/// </summary>
/// <typeparam name="TResult">Type of result.</typeparam>
internal class ApiResponseResult<TResult> : IResult
    where TResult : class
{
    private readonly ServiceMonitoringDefinition monitoringIds;

    private readonly ApiResponseBody<TResult>? bodyResult;

    private readonly ApiResponseBodyEmpty? emptyResult;

    private readonly ApiResponseResultState state;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiResponseResult{TResult}" /> class.
    /// </summary>
    /// <param name="monitoringIds">The application correlation ids.</param>
    /// <param name="result">The api body result.</param>
    public ApiResponseResult(ServiceMonitoringDefinition monitoringIds, ApiResponseBody<TResult> result)
    {
        this.monitoringIds = monitoringIds;
        this.state = result.Success;
        this.bodyResult = result;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiResponseResult{TResult}" /> class.
    /// </summary>
    /// <param name="monitoringIds">The application correlation ids.</param>
    /// <param name="result">The empty body result.</param>
    public ApiResponseResult(ServiceMonitoringDefinition monitoringIds, ApiResponseBodyEmpty result)
    {
        this.monitoringIds = monitoringIds;
        this.state = result.Success;
        this.emptyResult = result;
    }

    /// <summary>
    ///     Write an HTTP response reflecting the result and set headers with <see cref="ServiceMonitoringDefinition" />.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext" /> for the current request.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.Headers.Add(ServiceMonitoringDefinition.CorrelationIdKey, this.monitoringIds.CorrelationId.ToString());
        httpContext.Response.Headers.Add(ServiceMonitoringDefinition.FunctionalIdKey, this.monitoringIds.FunctionalId);
        httpContext.Response.Headers.Add(ServiceMonitoringDefinition.TechnicalIdKey, this.monitoringIds.TechnicalId.ToString());

        httpContext.Response.StatusCode = this.state switch
        {
            ApiResponseResultState.Success => (int)HttpStatusCode.OK,
            ApiResponseResultState.Incomplete => (int)HttpStatusCode.OK,
            ApiResponseResultState.Error => (int)HttpStatusCode.InternalServerError,
            ApiResponseResultState.BadRequest => (int)HttpStatusCode.BadRequest,
            ApiResponseResultState.NotFound => (int)HttpStatusCode.NotFound,
            ApiResponseResultState.Forbidden => (int)HttpStatusCode.Forbidden,
            ApiResponseResultState.Unauthorized => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };

        if (this.bodyResult is null)
        {
            await httpContext.Response.WriteAsJsonAsync(this.emptyResult);
        }
        else
        {
            if (this.bodyResult.Response is null && this.bodyResult.ResponseError is null)
                return;

            await httpContext.Response.WriteAsJsonAsync(this.bodyResult);
        }
    }
}
