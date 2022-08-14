namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

public class StudentAccessLayer : BaseAccessLayer<NecDbContext, Student>, IStudentAccessLayer
{
    public StudentAccessLayer(NecDbContext context)
        : base(context)
    {
    }
}
