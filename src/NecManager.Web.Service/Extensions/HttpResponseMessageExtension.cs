namespace NecManager.Web.Service.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using NecManager.Common;
using NecManager.Common.DataEnum.Internal;

/// <summary>
///     Provides extensions methods for <seealso cref="HttpResponseMessage" />.
/// </summary>
public static class HttpResponseMessageExtension
{
    /// <summary>
    ///     Build the api data service response based on <seealso cref="HttpResponseMessage" />.
    /// </summary>
    /// <typeparam name="TResponse">Type of object to read inside the <seealso cref="HttpResponseMessage.Content" />.</typeparam>
    /// <param name="httpResponseMessage"><seealso cref="HttpResponseMessage" /> concerned by the result to build.</param>
    /// <param name="validStatusCode"><seealso cref="HttpStatusCode" /> to consider that operation is in success.</param>
    /// <returns>
    ///     <list type="bullet">
    ///         <item>success : <seealso cref="bool" /> indicate whether the operation is success or not.</item>
    ///         <item>resultObj : <typeparamref name="TResponse" /> deserialized object from <seealso cref="HttpResponseMessage.Content" />.</item>
    ///         <item>errorMessage : <seealso cref="string" /> description of the error if something went wrong.</item>
    ///     </list>
    /// </returns>
    public static async Task<ServiceResult<TResponse>> BuildDataServiceResultAsync<TResponse>(
        this HttpResponseMessage httpResponseMessage, params HttpStatusCode[] validStatusCode)
        where TResponse : class
    {
        var (state, resultObj, errorMessage) = await httpResponseMessage.BuildServiceResultAsync<ApiResponseBody<TResponse>, RestServiceErrorCode>(validStatusCode).ConfigureAwait(false);

        return new(state, resultObj?.Response, errorMessage);
    }

    /// <summary>
    ///     Build the api data service response based on <seealso cref="HttpResponseMessage" /> without reading <seealso cref="HttpResponseMessage.Content" />.
    /// </summary>
    /// <param name="httpResponseMessage"><seealso cref="HttpResponseMessage" /> concerned by the result to build.</param>
    /// <param name="validStatusCode"><seealso cref="HttpStatusCode" /> to consider that operation is in success.</param>
    /// <returns>
    ///     <list type="bullet">
    ///         <item>success : <seealso cref="bool" /> indicate whether the operation is success or not.</item>
    ///         <item>errorMessage : <seealso cref="string" /> description of the error if something went wrong.</item>
    ///     </list>
    /// </returns>
    public static async Task<ServiceResult> BuildDataServiceResultAsync(this HttpResponseMessage httpResponseMessage, params HttpStatusCode[] validStatusCode)
    {
        var (state, _, errorMessage) = await httpResponseMessage.BuildServiceResultAsync<ApiResponseBodyEmpty, RestServiceErrorCode>(validStatusCode).ConfigureAwait(false);
        return new(state, errorMessage);
    }

    /// <summary>
    ///     Build the api data service response based on <seealso cref="HttpResponseMessage" />.
    /// </summary>
    /// <typeparam name="T">Type of object to read inside the <seealso cref="HttpResponseMessage.Content" />.</typeparam>
    /// <typeparam name="TResource">Type of resource from which to read the API error message.</typeparam>
    /// <param name="httpResponseMessage"><seealso cref="HttpResponseMessage" /> concerned by the result to build.</param>
    /// <param name="validStatusCodes"><seealso cref="HttpStatusCode" /> to consider that operation is in success.</param>
    /// <returns>
    ///     <list type="bullet">
    ///         <item>success : <seealso cref="bool" /> indicate whether the operation is success or not.</item>
    ///         <item>resultObj : <typeparamref name="T" /> deserialized object from <seealso cref="HttpResponseMessage.Content" />.</item>
    ///         <item>errorMessage : <seealso cref="string" /> description of the error if something went wrong.</item>
    ///     </list>
    /// </returns>
    private static async Task<(ServiceResultState Success, T? ResultObj, string ErrorMessage)> BuildServiceResultAsync<T, TResource>(this HttpResponseMessage httpResponseMessage, params HttpStatusCode[] validStatusCodes)
        where TResource : struct, Enum
    {
        // Something went wrong in api
        // we need to read technical message
        if ((!validStatusCodes.Any() && !httpResponseMessage.IsSuccessStatusCode) || (validStatusCodes.Any() && !validStatusCodes.Contains(httpResponseMessage.StatusCode)))
        {
            if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest || httpResponseMessage.StatusCode == HttpStatusCode.NotFound || httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                return (ServiceResultState.Warning, default, await httpResponseMessage.ReadErrorMessageAsync<TResource>().ConfigureAwait(false));

            return (ServiceResultState.Error, default, await httpResponseMessage.ReadErrorMessageAsync<TResource>().ConfigureAwait(false));
        }

        // If the status code is valid,
        // Read the response content to retrieve T data
        var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        var deserializedObject = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        if (deserializedObject is ApiResponseBody<CreationResult> { Response: { } } apiResponseBody)
        {
            apiResponseBody.Response.CreatedUri = httpResponseMessage.Headers.Location;
        }

        return (ServiceResultState.Success, deserializedObject, string.Empty);
    }
}
