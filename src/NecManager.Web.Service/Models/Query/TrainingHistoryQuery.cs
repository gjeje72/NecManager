namespace NecManager.Web.Service.Models.Query;
using System.Collections.Generic;
using System.Globalization;

using NecManager.Common;

public sealed class TrainingHistoryQuery : APageableQuery
{
    public int Id { get; set; }

    public string StudentId { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override Dictionary<string, string> QueryParameters
    {
        get
        {
            var queryParameters = base.QueryParameters;

            foreach (var (key, value) in this.QueryStudentsParameters)
            {
                if (!queryParameters.ContainsKey(key) && !string.IsNullOrEmpty(value))
                {
                    queryParameters.Add(key, value);
                }
            }

            return queryParameters;
        }
    }

    /// <summary>
    ///     Gets the query students parameters.
    /// </summary>
    private Dictionary<string, string> QueryStudentsParameters => new()
    {
        { nameof(this.Id), this.Id.ToString(CultureInfo.InvariantCulture) },
        { nameof(this.StudentId), this.StudentId },
    };
}
