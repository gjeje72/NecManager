namespace NecManager.Common;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using NecManager.Common.DataEnum.Internal;

/// <summary>
///     Provides extensions methods for <seealso cref="ErrorMessageFormatter" />
/// </summary>
public static class ErrorMessageFormatter
{
    /// <summary>
    ///     Retrieves localized string description of a technical code error in the <seealso cref="HttpResponseMessage" />.
    /// </summary>
    /// <typeparam name="T">Enum type that contain the error code.</typeparam>
    /// <param name="httpMessage">The http response message that contains the technical error code</param>
    /// <param name="args">args used to format the description</param>
    /// <returns>
    ///     Returns a formatted and localized string description of a technical code error.
    /// </returns>
    public static async Task<string> ReadErrorMessageAsync<T>(this HttpResponseMessage httpMessage, params object?[] args)
        where T : struct, Enum
    {
        if (httpMessage.IsSuccessStatusCode || httpMessage.Content == null)
        {
            return string.Empty;
        }

        var resultingErrorMessage = await httpMessage.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(resultingErrorMessage))
        {
            return httpMessage.StatusCode switch
            {
                HttpStatusCode.BadRequest => "Bad request.",
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => "You are not authorized to do this action.",
                HttpStatusCode.NotFound => "Module or action not found.",
                HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable => "Internal method call failure.",
                _ => "Internal method call failure.",
            };
        }

        try
        {
            var deserializedObject = JsonSerializer.Deserialize<ApiResponseBody<dynamic>>(resultingErrorMessage, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });


            var errorMessages = new StringBuilder();
            if (deserializedObject?.ResponseError is not null)
            {

                foreach (var (errorCode, errorMessage) in deserializedObject.ResponseError)
                {
                    var eatMapiErrorCode = (T)Enum.ToObject(typeof(T), errorCode);
                    var errorMessageResources = GetResource(eatMapiErrorCode)?.GetString(eatMapiErrorCode.ToString(), CultureInfo.CurrentCulture);

                    if (errorMessageResources is null)
                        errorMessageResources = string.IsNullOrEmpty(errorMessage) ? eatMapiErrorCode.ToString() : errorMessage;

                    var errorMessageResourcesFormatted = string.Format(CultureInfo.CurrentCulture, errorMessageResources, args);

                    errorMessages.AppendLine(CultureInfo.CurrentCulture, $"{errorMessageResourcesFormatted}");
                }

                resultingErrorMessage = errorMessages.ToString();
            }
        }
        catch (Exception)
        { }

        return resultingErrorMessage;
    }

    /// <summary>
    ///     Retrieves the <seealso cref="ResourceManager" /> according to <seealso cref="Enum" /> type.
    /// </summary>
    /// <typeparam name="T">Enum type of the error code.</typeparam>
    /// <param name="errorCodeValue">error code value.</param>
    /// <returns>
    ///     Returns the <seealso cref="ResourceManager" /> to use with the errorCodeValue
    /// </returns>
    private static ResourceManager? GetResource<T>(T errorCodeValue)
        where T : struct, Enum => errorCodeValue switch
        {
            RestServiceErrorCode _ => null,
            var _ => null
        };
}
