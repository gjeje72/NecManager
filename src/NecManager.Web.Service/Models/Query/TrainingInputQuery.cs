namespace NecManager.Web.Service.Models.Query;

using System;
using System.Collections.Generic;

using NecManager.Common;
using NecManager.Common.DataEnum;

/// <summary>
///     Class which represent a training query used to filter the training collection.
/// </summary>
public sealed class TrainingInputQuery : APageableQuery
{
    public DifficultyType? DifficultyType { get; set; }

    public WeaponType? WeaponType { get; set; }

    public int? GroupId { get; set; }

    public DateTime? Date { get; set; }

    public int? Season { get; set; }

    public int? StudentId { get; set; }

    public string? Filter { get; set; }

    public bool OnlyIndividual { get; set; }

    public string? MasterName { get; set; }



    /// <inheritdoc />
    protected override Dictionary<string, string> QueryParameters
    {
        get
        {
            var queryParameters = base.QueryParameters;

            foreach (var (key, value) in this.QueryTrainingsParameters)
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
    ///     Gets the query trainings parameters.
    /// </summary>
    private Dictionary<string, string> QueryTrainingsParameters => new()
    {
        { nameof(this.DifficultyType), this.DifficultyType.ToString() ?? string.Empty },
        { nameof(this.WeaponType), this.WeaponType.ToString() ?? string.Empty},
        { nameof(this.GroupId), this.GroupId.ToString() ?? string.Empty },
        { nameof(this.Date), this.Date.ToString() ?? string.Empty },
        { nameof(this.Season), this.Season.ToString() ?? string.Empty },
        { nameof(this.StudentId), this.StudentId.ToString() ?? string.Empty },
        { nameof(this.OnlyIndividual), this.OnlyIndividual.ToString() ?? string.Empty },
        { nameof(this.MasterName), this.MasterName ?? string.Empty },
        { nameof(this.Filter), this.Filter ?? string.Empty },
    };
}
