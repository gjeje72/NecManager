namespace NecManager.Common;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http.Extensions;

/// <summary>
///     Class which represent a query.
/// </summary>
public abstract class AQuery
{
    /// <summary>
    ///     Gets the query string.
    /// </summary>
    public string AsQueryParams
    {
        get
        {
            var queryBuilder = new QueryBuilder(this.QueryParameters.Where(p => p.Value != null!));
            return queryBuilder.ToString();
        }
    }

    /// <summary>
    ///     Gets the query parameters.
    /// </summary>
    protected abstract Dictionary<string, string> QueryParameters { get; }
}
