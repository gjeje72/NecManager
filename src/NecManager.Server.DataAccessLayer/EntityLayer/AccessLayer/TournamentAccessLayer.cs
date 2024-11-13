namespace NecManager.Server.DataAccessLayer.EntityLayer.AccessLayer;

using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;
using NecManager.Server.DataAccessLayer.Model;

internal sealed class TournamentAccessLayer : BaseAccessLayer<NecDbContext, Tournament>, ITournamentAccessLayer
{
    public TournamentAccessLayer(NecDbContext context)
    : base(context)
    {
    }
}
