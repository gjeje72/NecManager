namespace NecManager.Server.Api.Business.Modules.Tournament.Models
{
    using System;
    using System.Collections.Generic;
    using NecManager.Common.DataEnum;
    using NecManager.Server.DataAccessLayer.Model;

    public sealed class TournamentInfos
    {
        public DateTime Date { get; set; }

        public TournamentType Division { get; set; }

        public int ChampionId { get; set; }

        public int Rank2Id { get; set; }

        public int Rank3Id { get; set; }

        public int Rank4Id { get; set; }

        public int Rank5Id { get; set; }

        public List<MatchResultDto> Matchs { get; set; } = new List<MatchResultDto>();
    }
}
