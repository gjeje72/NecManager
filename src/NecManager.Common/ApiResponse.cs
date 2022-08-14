namespace NecManager.Common;

/// <summary>
///     Record which represent an api response.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
/// <param name="MonitoringIds">The monitoring ids.</param>
/// <param name="ResponseBody">The response body.</param>
public record ApiResponse<TResponse>(ServiceMonitoringDefinition MonitoringIds, ApiResponseBody<TResponse> ResponseBody)
    where TResponse : class;
