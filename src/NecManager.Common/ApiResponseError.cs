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

    public static class TrainingApiErrors
    {
        public static readonly ApiResponseError TrainingNotFound = new(RestServiceErrorCode.TrainingNotFound, "Training not found.");
        public static readonly ApiResponseError TrainingStudentNotFound = new(RestServiceErrorCode.TrainingStudentNotFound, "Training student not found.");
        public static readonly ApiResponseError TrainingBadRequest = new(RestServiceErrorCode.TrainingBadRequest, "Training input bad request.");
        public static readonly ApiResponseError TrainingCreationFailure = new(RestServiceErrorCode.TrainingCreationFailure, "Create a new training failed.");
        public static readonly ApiResponseError TrainingsCreationFailure = new(RestServiceErrorCode.TrainingsCreationFailure, "Create some new trainings failed.");
        public static readonly ApiResponseError TrainingDeletionFailure = new(RestServiceErrorCode.TrainingDeletionFailure, "Delete a training failed.");
        public static readonly ApiResponseError TrainingUpdateFailure = new(RestServiceErrorCode.TrainingUpdateFailure, "Update training failed.");
    }

    public static class GroupApiErrors
    {
        public static readonly ApiResponseError GroupDeletionFailure = new(RestServiceErrorCode.GroupDeletionFailure, "Delete a group failed.");
        public static readonly ApiResponseError GroupUpdateFailure = new(RestServiceErrorCode.GroupUpdateFailure, "Update a group failed.");
        public static readonly ApiResponseError GroupNotFound = new(RestServiceErrorCode.GroupNotFound, "Group not found.");

    }

    public static class StudentApiErrors
    {
        public static readonly ApiResponseError StudentNotFound = new(RestServiceErrorCode.StudentNotFound, "Student not found.");
        public static readonly ApiResponseError StudentUpdateFailure = new(RestServiceErrorCode.StudentUpdateFailure, "Update student failed.");
        public static readonly ApiResponseError StudentsUpdateCategoryFailure = new(RestServiceErrorCode.StudentsUpdateCategoryFailure, "Update students category failed.");
        public static readonly ApiResponseError StudentDeletionFailure = new(RestServiceErrorCode.StudentDeletionFailure, "Delete student failed.");
    }

    public static class Auth
    {
        public static readonly ApiResponseError AuthenticationFailed = new(RestServiceErrorCode.AuthenticationFailure, "Your email or password is invalid.");

        public static readonly ApiResponseError LoginUserBlocked = new(RestServiceErrorCode.UserResourceBlocked, "Can't SignIn, the user has been desactivated. Please contact an administrator.");
    }

    public static readonly ApiResponseError RefreshTokenError = new(RestServiceErrorCode.UserResourceRefreshTokenBadRequest, "Incorrect details provided to refresh the user Token.");

}
