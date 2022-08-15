namespace NecManager.Common;

using NecManager.Common.DataEnum.Internal;

/// <summary>
///     Record which represent a return.
/// </summary>
/// <typeparam name="TResult">The result type.</typeparam>
/// <param name="State">A boolean indicating whether is success.</param>
/// <param name="Result">The result.</param>
/// <param name="ErrorMessage">The error message.</param>
public record ServiceResult<TResult>(ServiceResultState State, TResult? Result, params string[] ErrorMessage)
{
    /// <summary>
    ///     Method which retrieve all errors messages with comma as separator.
    /// </summary>
    /// <returns>Returns all errors messages with comma as separator.</returns>
    public string GetFormattedMessages() => string.Join(", ", this.ErrorMessage);
}
