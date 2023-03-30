namespace NecManager.Common;
using System.Collections.Generic;

/// <summary>
///     Class which represent a pageable result.
/// </summary>
/// <typeparam name="TItem">Type of result elements.</typeparam>
public sealed class PageableResult<TItem>
{
    /// <summary>
    ///     Gets or sets the list of elements.
    /// </summary>
    public IEnumerable<TItem>? Items { get; set; }

    /// <summary>
    ///     Gets or sets the total elements.
    /// </summary>
    public int TotalElements { get; set; }
}
