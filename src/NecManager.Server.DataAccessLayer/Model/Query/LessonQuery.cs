namespace NecManager.Server.DataAccessLayer.Model.Query;

using NecManager.Common;
using NecManager.Common.DataEnum;

/// <summary>
///     Record which represent pageable lesson query.
/// </summary>
/// <param name="PageSize">The page size.</param>
/// <param name="CurrentPage">The current page.</param>
public sealed record LessonQuery(int PageSize, int CurrentPage, DifficultyType? DifficultyType = null, WeaponType? WeaponType = null, int? GroupId = null) : PageableQuery(PageSize, CurrentPage);
