namespace NecManager.Web.Service.Models.Query;

using System.Collections.Generic;

using NecManager.Common;
using NecManager.Common.DataEnum;

/// <summary>
///     Class which represent a lesson query used to filter the lesson collection.
/// </summary>
public sealed class LessonInputQuery : APageableQuery
{
    public DifficultyType? DifficultyType { get; set; }

    public WeaponType? WeaponType { get; set; }

    public int? GroupId { get; set; }

    /// <inheritdoc />
    protected override Dictionary<string, string> QueryParameters
    {
        get
        {
            var queryParameters = base.QueryParameters;

            foreach (var (key, value) in this.QueryLessonsParameters)
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
    ///     Gets the query products parameters.
    /// </summary>
    private Dictionary<string, string> QueryLessonsParameters => new()
    {
        { nameof(this.DifficultyType), this.DifficultyType.ToString() ?? string.Empty },
        { nameof(this.WeaponType), this.WeaponType.ToString() ?? string.Empty},
        { nameof(this.GroupId), this.GroupId.ToString() ?? string.Empty },
    };
}
