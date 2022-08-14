namespace NecManager.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NecManager.Common.DataEnum.Internal;

/// <summary>
///     Record which represent a api error.
/// </summary>
/// <param name="ErrorCode">The error code.</param>
/// <param name="ErrorMessage">The error message.</param>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Explicit with error messages.")]
public record ApiResponseError(RestServiceErrorCode ErrorCode, string ErrorMessage)
{
    //public static readonly ApiResponseError Empty = new(RestServiceErrorCode.ApiDataUnavailable, string.Empty);
}
