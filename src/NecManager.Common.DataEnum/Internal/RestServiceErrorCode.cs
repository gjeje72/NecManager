namespace NecManager.Common.DataEnum.Internal;
public enum RestServiceErrorCode
{
    AuthenticationFailure = 7,

    UserResourceBlocked = 115,
    UserResourceRefreshTokenBadRequest = 125,

    CreateError = 1000,

    GetError = 2000,

    LessonNotFound = 3000,
    LessonBadRequest = 3001,
    LessonCreationFailure = 3002,
    LessonDeletionFailure = 3003,
    LessonUpdateFailure = 3004,

    TrainingNotFound = 4000,
    TrainingBadRequest = 4001,
    TrainingCreationFailure = 4002,
    TrainingDeletionFailure = 4003,
    TrainingUpdateFailure = 4004,
    TrainingsCreationFailure = 4005,
    TrainingStudentNotFound = 4006,

    StudentNotFound = 5001,
    StudentUpdateFailure = 5002,
    StudentDeletionFailure = 5003,
    StudentsUpdateCategoryFailure = 5004,

    GroupDeletionFailure = 6000,
    GroupNotFound = 6001,
    GroupUpdateFailure = 6002,
}
