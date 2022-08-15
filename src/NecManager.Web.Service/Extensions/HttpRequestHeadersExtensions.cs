namespace NecManager.Web.Service.Extensions;
using System.Net.Http.Headers;

using NecManager.Common;

/// <summary>
///     Extension class eof <see cref="HttpRequestHeaders" />.
/// </summary>
internal static class HttpRequestHeadersExtensions
{
    /// <summary>
    ///     Method which add <see cref="ServiceMonitoringDefinition" /> values to http headers.
    /// </summary>
    /// <param name="headers">The http headers.</param>
    /// <param name="monitoringIds">The monitoring ids.</param>
    public static void AddMonitoringIds(this HttpRequestHeaders headers, ServiceMonitoringDefinition monitoringIds)
    {
        var (correlationId, functionalId, technicalId) = monitoringIds;
        headers.Add(ServiceMonitoringDefinition.FunctionalIdKey, functionalId);
        headers.Add(ServiceMonitoringDefinition.CorrelationIdKey, correlationId.ToString());
        headers.Add(ServiceMonitoringDefinition.TechnicalIdKey, technicalId.ToString());
    }
}
