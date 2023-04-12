namespace NecManager.Server.DataAccessLayer.Model.Query;

using NecManager.Common;
using NecManager.Common.DataEnum;

public sealed record StudentQuery(
    int PageSize,
    int CurrentPage,
    WeaponType? WeaponType = null,
    int? GroupId = null,
    StudentState? State = null) : PageableQuery(PageSize, CurrentPage);
