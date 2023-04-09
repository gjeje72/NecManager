namespace NecManager.Server.DataAccessLayer.Model.Query;
using NecManager.Common.DataEnum;


using NecManager.Common;

/// <summary>
///     Record which represent pageable lesson query.
/// </summary>
/// <param name="PageSize">The page size.</param>
/// <param name="CurrentPage">The current page.</param>
public sealed record TrainingQuery(
    int PageSize,
    int CurrentPage,
    DifficultyType? DifficultyType = null,
    WeaponType? WeaponType = null,
    int? GroupId = null,
    DateTime? Date = null,
    int? Season = null,
    int? StudentId = null,
    bool OnlyIndividual = false,
    string? MasterName = null) : PageableQuery(PageSize, CurrentPage);
