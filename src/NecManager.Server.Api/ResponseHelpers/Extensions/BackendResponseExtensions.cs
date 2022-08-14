namespace NecManager.Server.Api.ResponseHelpers.Extensions;

using NecManager.Common;

/// <summary>
/// Static class which contains <see cref="IResultExtensions"/> extensions methods.
/// </summary>
public static class BackendResponseExtensions
{
    /// <summary>
    ///     Method which returns a new api response result.
    /// </summary>
    /// <typeparam name="TResult">Type of result.</typeparam>
    /// <param name="resultExtensions">The result extensions.</param>
    /// <param name="result">The result.</param>
    /// <returns>Returns a new api response result.</returns>
    public static IResult ApiResponse<TResult>(this IResultExtensions resultExtensions, ApiResponse<TResult> result)
        where TResult : class
    {
        ArgumentNullException.ThrowIfNull(resultExtensions, nameof(resultExtensions));

        var (apiRequestHeaders, apiResponseBody) = result;
        return new ApiResponseResult<TResult>(apiRequestHeaders, apiResponseBody);
    }

    /// <summary>
    ///     Method which returns a new api response result.
    /// </summary>
    /// <param name="resultExtensions">The result extensions.</param>
    /// <param name="result">The result.</param>
    /// <returns>Returns a new api response result.</returns>
    public static IResult ApiResponseEmpty(this IResultExtensions resultExtensions, ApiResponseEmpty result)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions, nameof(resultExtensions));

        var (apiRequestHeaders, apiResponseBody) = result;
        return new ApiResponseResult<string>(apiRequestHeaders, apiResponseBody);
    }
}
