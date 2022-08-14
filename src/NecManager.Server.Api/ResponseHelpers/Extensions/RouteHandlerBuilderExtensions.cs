namespace NecManager.Server.Api.ResponseHelpers.Extensions;

using NecManager.Common;

/// <summary>
///     Static class which contains <see cref="RouteHandlerBuilder" /> extensions methods.
/// </summary>
internal static class RouteHandlerBuilderExtensions
{
    /// <summary>
    ///     Method which define <see cref="ApiResponseBody{TResponse}" /> as endpoint returns.
    /// </summary>
    /// <typeparam name="TResult">The result body type.</typeparam>
    /// <param name="routeHandlerBuilder">The route handler builder.</param>
    /// <returns>Returns the route handler builder</returns>
    public static RouteHandlerBuilder ProducesApiResponse<TResult>(this RouteHandlerBuilder routeHandlerBuilder)
        where TResult : class
        => routeHandlerBuilder.Produces<ApiResponseBody<TResult>>();

    /// <summary>
    ///     Method which define <see cref="ApiResponseBodyEmpty" /> as endpoint returns.
    /// </summary>
    /// <param name="routeHandlerBuilder">The route handler builder.</param>
    /// <returns>Returns the route handler builder</returns>
    public static RouteHandlerBuilder ProducesApiResponseEmpty(this RouteHandlerBuilder routeHandlerBuilder)
        => routeHandlerBuilder.Produces<ApiResponseBodyEmpty>();
}
