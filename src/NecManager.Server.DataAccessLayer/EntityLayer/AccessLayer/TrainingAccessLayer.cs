namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

public sealed class TrainingAccessLayer : BaseAccessLayer<NecDbContext, Training>, ITrainingAccessLayer
{
    public TrainingAccessLayer(NecDbContext context)
        : base(context)
    {
    }
}
