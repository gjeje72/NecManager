namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using Microsoft.EntityFrameworkCore;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions.Query;
using NecManager.Server.DataAccessLayer.Model;
using NecManager.Server.DataAccessLayer.Model.Query;

public sealed class TrainingAccessLayer : AQueryBaseAccessLayer<NecDbContext, Training, TrainingQuery>, ITrainingAccessLayer
{
    public TrainingAccessLayer(NecDbContext context)
        : base(context)
    {
    }

    /// <inheritdoc />
    protected override async Task<IEnumerable<Training>> GetCollectionInternalAsync(TrainingQuery query, bool isPageable = true)
    {
        IQueryable<Training> queryable = this.ModelSet.Include(t => t.Lesson).Include(t => t.PersonTrainings).ThenInclude(pt => pt.Student).Include(t => t.Group);

        if (query.Date is not null)
            queryable = queryable.Where(t => t.Date == query.Date);

        if (query.Season is not null)
        {
            var seasonStartDate = new DateTime((int)query.Season - 1, 9, 1);
            var seasonEndDate = new DateTime((int)query.Season, 8, 31);
            queryable = queryable.Where(t => t.Date >= seasonStartDate && t.Date < seasonEndDate);
        }

        if (query.GroupId is not null)
            queryable = queryable.Where(t => t.GroupId == query.GroupId);

        if (query.StudentId is not null)
            queryable = queryable.Where(t => t.PersonTrainings.Any(pt => pt.StudentId == query.StudentId));

        if (query.OnlyIndividual)
            queryable = queryable.Where(t => t.PersonTrainings.Any(pt => pt.IsIndividual == query.OnlyIndividual));

        if (query.DifficultyType is not null)
            queryable = queryable.Where(t => t.Lesson != null && t.Lesson.Difficulty == query.DifficultyType);

        if (query.WeaponType is not null)
            queryable = queryable.Where(t => t.Lesson != null && t.Lesson.Weapon == query.WeaponType);

        if (!string.IsNullOrWhiteSpace(query.MasterName))
            queryable = queryable.Where(t => t.MasterName != null && t.MasterName.Contains(query.MasterName));

        if(!string.IsNullOrWhiteSpace(query.Filter))
            queryable = queryable.Where(t => t.Lesson!.Title.Contains(query.Filter)
                                                 || t.Lesson.Description.Contains(query.Filter)
                                                 || t.Group!.Title.Contains(query.Filter)
                                                 || t.PersonTrainings.Any(pt => pt.Student!.Name.Contains(query.Filter))
                                                 || t.PersonTrainings.Any(pt => pt.Student!.FirstName.Contains(query.Filter))
                                                 );

        queryable = queryable.OrderBy(t => t.Date);
        var collectionInternal = !isPageable ? queryable : queryable.Skip((query.CurrentPage - 1) * query.PageSize).Take(query.PageSize);
        return await collectionInternal.ToListAsync();
    }
}
