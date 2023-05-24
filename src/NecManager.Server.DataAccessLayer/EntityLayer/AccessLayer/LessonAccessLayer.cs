namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions.Query;
using NecManager.Server.DataAccessLayer.Model;
using NecManager.Server.DataAccessLayer.Model.Query;

public sealed class LessonAccessLayer : AQueryBaseAccessLayer<NecDbContext, Lesson, LessonQuery>, ILessonAccessLayer
{
    public LessonAccessLayer(NecDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    protected override IQueryable<Lesson> GetCollectionInternal(LessonQuery query, bool isPageable = true)
    {
        IQueryable<Lesson> queryable = this.ModelSet.Include(l => l.Trainings);

        if (query.GroupId is not null)
            queryable = queryable.Where(l => l.Trainings.Any() && l.Trainings.Any(t => t.GroupId == query.GroupId));

        if (query.DifficultyType is not null)
            queryable = queryable.Where(l => l.Difficulty == query.DifficultyType);

        if (query.WeaponType is not null)
            queryable = queryable.Where(l => l.Weapon == query.WeaponType);

        var collectionInternal = !isPageable ? queryable : queryable.Skip((query.CurrentPage - 1) * query.PageSize).Take(query.PageSize);
        return collectionInternal;
    }
}
