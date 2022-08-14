namespace NecManager.Common;

/// <summary>
///     Record which represent an api response.
/// </summary>
/// <param name="MonitoringIds">The monitoring ids.</param>
/// <param name="ResponseBody">The response body.</param>
public record ApiResponseEmpty(ServiceMonitoringDefinition MonitoringIds, ApiResponseBodyEmpty ResponseBody);
