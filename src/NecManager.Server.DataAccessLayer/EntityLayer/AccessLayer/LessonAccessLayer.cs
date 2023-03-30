namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using System.Collections.Generic;
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
    protected override async Task<IEnumerable<Lesson>> GetCollectionInternalAsync(LessonQuery query, bool isPageable = true)
    {
        IQueryable<Lesson> whereQueryable = this.ModelSet;

        var collectionInternal = !isPageable ? whereQueryable : whereQueryable.Skip((query.CurrentPage - 1) * query.PageSize).Take(query.PageSize);
        return await collectionInternal.ToListAsync();
    }
}
