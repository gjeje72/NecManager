namespace NecManager.Server.Api.Business.Modules.Tournament;

using NecManager.Common.DataEnum;
using NecManager.Server.Api.Business.Modules.Tournament.Models;
using NecManager.Server.DataAccessLayer.EntityLayer.Abstractions;

internal sealed class EngineTournament
{
    private readonly ITournamentAccessLayer tournamentAccessLayer;

    public EngineTournament(ITournamentAccessLayer tournamentAccessLayer)
    {
        this.tournamentAccessLayer = tournamentAccessLayer;
    }
    public TournamentInfos? GetNewTournament (TournamentType division, Dictionary<int, int> absents)
    {
        // récupère le dernier tournoi
        var lastTournament = this.tournamentAccessLayer.GetCollection(filter: t => t.Division == division).OrderByDescending(t => t.Date).FirstOrDefault();

        if (lastTournament == null)
            return null;

        var result = new TournamentInfos
        {
            Date = DateTime.UtcNow,
            Division = division,
        };
        var classement = new List<int>
        {
            lastTournament.ChampionId,
            lastTournament.Rank2Id,
            lastTournament.Rank3Id,
            lastTournament.Rank4Id,
            lastTournament.Rank5Id,
        };

        // relégation des absents
        var absentsToRelegate = absents.Where(kvp => kvp.Value >= 2).ToDictionary(x => x.Key, x => x.Value);
        var absentsToRelegateCount = absentsToRelegate.Count();
        if (absentsToRelegateCount > 0)
        {
            var firstId = absentsToRelegate.First().Key;
            var firstIndex = classement.IndexOf(firstId);

            if (absentsToRelegateCount == 2)
            {
                var secondId = absentsToRelegate[1];
                var secondIndex = classement.IndexOf(secondId);
                var isConsecutif = Math.Abs(firstIndex - secondIndex) == 1;

            }
            else
            {

            }

            //if (firstIndex == 4 || firstIndex == 3 && absentsToRelegate.ContainsKey(classement[firstIndex + 1]))
            //{
            //    // aucun changements
            //}
            //else
            //{
                
            //}

            //if (firstIndex < 3 && absentsToRelegate.ContainsKey(classement[firstIndex + 1]))
            //{
                
            //}
            //else
            //{
            //    var up = classement[firstIndex + 1];
            //    classement[firstIndex] = up;
            //    classement[firstIndex + 1] = firstId;
            //}
            
        }
        // créer les matchs en fonction des absents 

        return result;
    }



    // UploadResult (TournamentInfos : (TournamentType division, DateTime date, MatchsResult[] matchs + nouveau classement)
    // UploadManyResults (TournamentInfos[])
}
