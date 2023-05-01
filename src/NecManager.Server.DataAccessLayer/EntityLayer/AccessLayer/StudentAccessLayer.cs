namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions.Query;
using NecManager.Server.DataAccessLayer.Model;
using NecManager.Server.DataAccessLayer.Model.Query;

public class StudentAccessLayer : AQueryBaseAccessLayer<NecDbContext, Student, StudentQuery>, IStudentAccessLayer
{
    public StudentAccessLayer(NecDbContext context)
        : base(context)
    {
    }

    protected override async Task<IEnumerable<Student>> GetCollectionInternalAsync(StudentQuery query, bool isPageable = true)
    {
        IQueryable<Student> queryable = this.ModelSet.Include(l => l.StudentGroups).ThenInclude(sg => sg.Group);

        if (query.State is not null)
            queryable = queryable.Where(l => l.State == query.State);

        if (query.GroupId is not null)
            queryable = queryable.Where(l => l.StudentGroups.Any() && l.StudentGroups.Any(t => t.GroupId == query.GroupId));

        if (query.WeaponType is not null)
            queryable = queryable.Where(l => l.StudentGroups.Any() && l.StudentGroups.Any(sg => sg.Group != null && sg.Group.Weapon == query.WeaponType));

        if (!string.IsNullOrWhiteSpace(query.Filter))
            queryable = queryable.Where(s => s.Name.Contains(query.Filter)
                                                 || s.FirstName.Contains(query.Filter)
                                                 || s.EmailAddress.Contains(query.Filter)
                                                 || s.StudentGroups.Any(sg => sg.Group != null && sg.Group.Title.Contains(query.Filter))
                                                 );
        queryable = queryable.OrderBy(s => s.Name);
        var collectionInternal = !isPageable ? queryable : queryable.Skip((query.CurrentPage - 1) * query.PageSize).Take(query.PageSize);
        return await collectionInternal.ToListAsync();
    }
}
