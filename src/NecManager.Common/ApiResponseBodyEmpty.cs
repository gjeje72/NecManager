namespace NecManager.Common;

using System;

using NecManager.Common.DataEnum.Internal;

/// <summary>
///     Record which represent empty api response body.
/// </summary>
public record ApiResponseBodyEmpty(ApiResponseResultState Success, params ApiResponseError[] ResponseError)
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiResponseBodyEmpty" /> class.
    /// </summary>
    public ApiResponseBodyEmpty()
        : this(ApiResponseResultState.Success)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiResponseBodyEmpty" /> class.
    /// </summary>
    /// <param name="resultState">The result state.</param>
    public ApiResponseBodyEmpty(ApiResponseResultState resultState)
        : this(resultState, Array.Empty<ApiResponseError>())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiResponseBodyEmpty" /> class.
    /// </summary>
    /// <param name="responseError">The <see cref="ApiResponseError" />.</param>
    public ApiResponseBodyEmpty(params ApiResponseError[] responseError)
        : this(ApiResponseResultState.Error, responseError)
    {
    }
}
