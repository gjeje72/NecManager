namespace NecManager.Web.Service.Models.Query;

using System.Collections.Generic;

using NecManager.Common;
using NecManager.Common.DataEnum;

/// <summary>
///     Class which represent a student query used to filter the student collection.
/// </summary>
public sealed class StudentInputQuery : APageableQuery
{
    public string? Filter { get; set; }

    public bool? IsPageable { get; set; }

    public WeaponType? WeaponType { get; set; }

    public StudentState? StudentState { get; set; }

    public int? GroupId { get; set; }

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
        { nameof(this.Filter), this.Filter ?? string.Empty },
        { nameof(this.WeaponType), this.WeaponType.ToString() ?? string.Empty},
        { nameof(this.StudentState), this.StudentState.ToString() ?? string.Empty},
        { nameof(this.GroupId), this.GroupId.ToString() ?? string.Empty },
        { nameof(this.IsPageable), this.IsPageable.ToString() ?? string.Empty },
    };
}
