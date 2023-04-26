namespace NecManager.Common;

using System.Collections.Generic;
using System.Globalization;

/// <summary>
///     Class which represent pageable query.
/// </summary>
public abstract class APageableQuery : AQuery
{
    /// <summary>
    ///     Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    ///     Gets or sets the current page.
    /// </summary>
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    ///     Gets the query parameters.
    /// </summary>
    protected override Dictionary<string, string> QueryParameters => this.QueryPageableParameters;

    /// <summary>
    ///     Gets the dictionary of pageable parameters.
    /// </summary>
    private Dictionary<string, string> QueryPageableParameters
        => new()
        {
            { nameof(this.PageSize), this.PageSize.ToString(CultureInfo.InvariantCulture) },
            { nameof(this.CurrentPage), this.CurrentPage.ToString(CultureInfo.InvariantCulture) },
        };
}
