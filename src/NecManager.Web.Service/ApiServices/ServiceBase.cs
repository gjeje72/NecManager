namespace NecManager.Web.Service.ApiServices;

using NecManager.Web.Service.Provider;

/// <summary>
///     Class which represent a service base.
/// </summary>
internal abstract class ServiceBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ServiceBase" /> class.
    /// </summary>
    /// <param name="restHttpService">The rest http client provider.</param>
    /// <param name="functionalMonitoringId">The functional id.</param>
    protected ServiceBase(RestHttpService restHttpService, string functionalMonitoringId)
    {
        this.RestHttpService = restHttpService;
        this.RestHttpService.SetServiceMonitoringFunctionalId(functionalMonitoringId);
    }

    /// <summary>
    ///     Gets the rest http client provider.
    /// </summary>
    protected RestHttpService RestHttpService { get; }
}
