namespace NecManager.Server.DataAccessLayer.Model;

using System;

using NecManager.Common.DataEnum;
using NecManager.Server.DataAccessLayer.Model.Abstraction;

public sealed class Tournament : ADataObject
{
    public DateTime Date { get; set; }

    public TournamentType Division { get; set; }

    public ICollection<MatchResult> Matchs { get; set; }
}
