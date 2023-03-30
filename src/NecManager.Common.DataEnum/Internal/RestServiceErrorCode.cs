namespace NecManager.Common.DataEnum.Internal;
public enum RestServiceErrorCode
{
    CreateError = 1000,

    GetError = 2000,

    LessonNotFound = 3000,
    LessonBadRequest = 3001,
    LessonCreationFailure = 3002,
    LessonDeletionFailure = 3003,
    LessonUpdateFailure = 3004,
}
