namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

public sealed class LessonAccessLayer : BaseAccessLayer<NecDbContext, Lesson>, ILessonAccessLayer
{
    public LessonAccessLayer(NecDbContext context)
        : base(context)
    {
    }
}
