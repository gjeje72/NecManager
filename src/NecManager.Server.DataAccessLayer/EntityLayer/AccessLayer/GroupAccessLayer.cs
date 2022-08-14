namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

public class GroupAccessLayer : BaseAccessLayer<NecDbContext, Group>, IGroupAccessLayer
{
    public GroupAccessLayer(NecDbContext context)
        : base(context)
    {
    }
}
