namespace NecManager.Server.DataAccessLayer.Model;

using NecManager.Common.DataEnum;
using NecManager.Server.DataAccessLayer.Model.Abstraction;

public sealed class MatchResult : ADataObject
{
    public int Winner { get; set; }

    public int Looser { get; set; }

    public int WinnerScore { get; set; }

    public int LooserScore { get; set; }

    public int Target { get; set; }

    public int TournamentId { get; set; }

    public TournamentType TournamentType { get; set; }

    public Round TournamentRound { get; set; }
}
