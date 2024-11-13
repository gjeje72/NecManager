namespace NecManager.Server.Api.Business.Modules.Tournament.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NecManager.Common.DataEnum;

    public sealed class MatchResultDto
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
}
