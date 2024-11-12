namespace NecManager.Server.DataAccessLayer.Model;

using System;

using NecManager.Common.DataEnum;
using NecManager.Server.DataAccessLayer.Model.Abstraction;

public sealed class Tournament : ADataObject
{
    public DateTime Date { get; set; }

    public TournamentType Division { get; set; }

    public int ChampionId { get; set; }

    public int Rank2Id { get; set; }

    public int Rank3Id { get; set; }

    public int Rank4Id { get; set; }

    public int Rank5Id { get; set; }

    mettre à jour la migration !

    public ICollection<MatchResult> Matchs { get; set; } = new HashSet<MatchResult>();
}
