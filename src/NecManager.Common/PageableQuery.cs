namespace NecManager.Common;

/// <summary>
///     Class which represent pageable query.
/// </summary>
/// <param name="PageSize">Gets or sets the page size.</param>
/// <param name="CurrentPage">Gets or sets the current page.</param>
public record PageableQuery(int PageSize, int CurrentPage);
