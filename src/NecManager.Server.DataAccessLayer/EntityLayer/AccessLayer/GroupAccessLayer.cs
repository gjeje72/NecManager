namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

public class GroupAccessLayer : BaseAccessLayer<NecLiteDbContext, Group>, IGroupAccessLayer
{
    public GroupAccessLayer(NecLiteDbContext context)
        : base(context)
    {
    }
}
