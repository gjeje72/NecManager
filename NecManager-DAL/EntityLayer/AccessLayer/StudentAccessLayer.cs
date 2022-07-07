namespace NecManager_DAL.EntityLayer.AccessLayer;

using NecManager_DAL.Model;

public class StudentAccessLayer : BaseAccessLayer<AppDbContext, Student>
{
    public StudentAccessLayer(AppDbContext appDbContext)
        : base(appDbContext)
    {
    }
}
