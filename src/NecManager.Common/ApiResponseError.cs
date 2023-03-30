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
    public static class LessonApiErrors
    {
        public static readonly ApiResponseError LessonNotFound = new(RestServiceErrorCode.LessonNotFound, "Lesson not found.");
        public static readonly ApiResponseError LessonBadRequest = new(RestServiceErrorCode.LessonBadRequest, "Lesson input bad request.");
        public static readonly ApiResponseError LessonCreationFailure = new(RestServiceErrorCode.LessonCreationFailure, "Create a new lesson failed.");
        public static readonly ApiResponseError LessonDeletionFailure = new(RestServiceErrorCode.LessonDeletionFailure, "Delete a lesson failed.");
        public static readonly ApiResponseError LessonUpdateFailure = new(RestServiceErrorCode.LessonUpdateFailure, "Update a lesson failed.");
    }
}
