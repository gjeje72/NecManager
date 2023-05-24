namespace NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Query;

using NecManager.Common;
using NecManager.Server.DataAccessLayer.Model;
using NecManager.Server.DataAccessLayer.Model.Query;

public interface IStudentAccessLayer
{
    Task<string> AddAsync(Student model);
    Task<int> AddRangeAsync(IEnumerable<Student> students);
    Task<PageableResult<Student>> GetPageableCollectionAsync(StudentQuery query, bool isPageable);
    Task<Student?> GetSingleAsync(Expression<Func<Student, bool>>? filter = null, bool trackingEnabled = false, Func<IQueryable<Student>, IIncludableQueryable<Student, object>>? navigationProperties = null);
    Task<int> UpdateAsync(Student model);
    Task<int> UpdateListAsync(IEnumerable<Student> models);
    Task<int> RemoveAsync(string id);
    Task<int> RemoveAsync(Student model);
    Task<int> RemoveRangeAsync(IEnumerable<string> ids);
    Task<int> RemoveRangeAsync(IEnumerable<Student> models);
    Task<bool> ExistsAsync(string id);
    Task<bool> ExistsRangeAsync(IEnumerable<string> ids);
}
