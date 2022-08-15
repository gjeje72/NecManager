namespace NecManager.Common;

using NecManager.Common.DataEnum.Internal;

/// <summary>
///     Record which represent a return.
/// </summary>
public record ServiceResult(ServiceResultState State, params string[] ErrorMessage)
    : ServiceResult<object>(State, null, ErrorMessage);
