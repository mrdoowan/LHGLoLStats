using System;
using System.Collections.Generic;
using RiotSharp.Endpoints.MatchEndpoint;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    public class Team {

        // Non-DB Columns member variables/Functions
        private decimal MinuteDuration;
        public HashSet<string> ParticipantIds { get; private set; }
        public int GetTotalKills() {
            return (int)Players.GetTeamTotalStat(TeamStat.KILLS);
        }
        public int GetTotalDeaths() {
            return (int)Players.GetTeamTotalStat(TeamStat.DEATHS);
        }
        public int GetTotalAssists() {
            return (int)Players.GetTeamTotalStat(TeamStat.ASSISTS);
        }

        // Default Ctor
        public Team(int sideID) {
            Side = sideID;
            Players = new PlayerList();
            Objectives = new ObjectiveList();
            ParticipantIds = new HashSet<string>();
        }

        #region DB Columns

        public int Side { get; set; }
        public int TeamId { get; set; }
        public bool Win { get; private set; }
        public bool FirstBlood { get; private set; }
        public bool FirstTower { get; private set; }
        public bool FirstDragon { get; private set; }
        public bool FirstRiftHerald { get; private set; }
        public List<Champ> Bans { get; private set; }
        public PlayerList Players { get; set; }
        public ObjectiveList Objectives { get; private set; }
        public decimal GetTotalKDA() {
            int totalKills = GetTotalKills();
            int totalDeaths = GetTotalDeaths();
            int totalAssists = GetTotalAssists();
            if (totalKills + totalAssists == 0) { return 0; }
            else if (totalDeaths == 0) { return -1; }
            decimal totalKDA = (decimal)(totalKills + totalAssists) / totalDeaths;
            return Math.Round(totalKDA, 2, MidpointRounding.AwayFromZero);
        }
        public decimal GetDamageToChampsPerMinute() {
            decimal total = Players.GetTeamTotalStat(TeamStat.DAMAGE_CHAMPS);
            return Math.Round(total / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }

        public decimal GetDamageToObjectivesPerMinute() {
            decimal total = Players.GetTeamTotalStat(TeamStat.DAMAGE_OBJECTIVES);
            return Math.Round(total / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }
        public decimal GetGoldPerMinute() {
            decimal total = Players.GetTeamTotalStat(TeamStat.GOLD);
            return Math.Round(total / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }
        public decimal GetVisionScorePerMinute() {
            decimal total = Players.GetTeamTotalStat(TeamStat.VISION_SCORE);
            return Math.Round(total / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }
        public int GetGoldAt15() {
            return (int)Players.GetTeamTotalStat(TeamStat.GOLD_AT_15);
        }
        public int GetGoldDiff15() {
            return (int)Players.GetTeamTotalStat(TeamStat.GOLD_DIFF_15);
        }
        public int GetXPAt15() {
            return (int)Players.GetTeamTotalStat(TeamStat.XP_AT_15);
        }
        public int GetXPDiff15() {
            return (int)Players.GetTeamTotalStat(TeamStat.XP_DIFF_15);
        }
        public int GetTowersAt15() {
            return Objectives.TowersAt15;
        }
        public int GetTowersDiff15() {
            return Objectives.TowersDiff15;
        }
        public int GetKillsAt15() {
            return Objectives.KillsAt15;
        }
        public int GetKillsDiff15() {
            return Objectives.KillsDiff15;
        }
        public int? GetGoldAt25() {
            return (MinuteDuration < MasterWrapper.MINUTE_25) ? (int?)null : (int)Players.GetTeamTotalStat(TeamStat.GOLD_AT_25);
        }
        public int? GetGoldDiff25() {
            return (MinuteDuration < MasterWrapper.MINUTE_25) ? (int?)null : (int)Players.GetTeamTotalStat(TeamStat.GOLD_DIFF_25);
        }
        public int? GetXPAt25() {
            return (MinuteDuration < MasterWrapper.MINUTE_25) ? (int?)null : (int)Players.GetTeamTotalStat(TeamStat.XP_AT_25);
        }
        public int? GetXPDiff25() {
            return (MinuteDuration < MasterWrapper.MINUTE_25) ? (int?)null : (int)Players.GetTeamTotalStat(TeamStat.XP_DIFF_25);
        }
        public int? GetTowersAt25() {
            return (MinuteDuration < MasterWrapper.MINUTE_25) ? (int?)null : Objectives.TowersAt25;
        }
        public int? GetTowersDiff25() {
            return (MinuteDuration < MasterWrapper.MINUTE_25) ? (int?)null : Objectives.TowersDiff25;
        }
        public int? GetKillsAt25() {
            return (MinuteDuration < MasterWrapper.MINUTE_25) ? (int?)null : Objectives.KillsAt25;
        }
        public int? GetKillsDiff25() {
            return (MinuteDuration < MasterWrapper.MINUTE_25) ? (int?)null : Objectives.KillsDiff25;
        }

        #endregion

        public void InitializeClass(TeamStats teamObj, int teamId, int duration, string patch) {
            TeamId = teamId;
            long seconds = duration % 60;
            MinuteDuration = (duration / 60) + ((decimal)seconds / 60);
            Win = (teamObj.Win == "Win") ? true : false;
            FirstBlood = teamObj.FirstBlood;
            FirstTower = teamObj.FirstTower;
            FirstDragon = teamObj.FirstDragon;
            FirstRiftHerald = teamObj.FirstRiftHerald;
            Bans = GetTeamBans(teamObj.Bans);
            Objectives.UpdateBaronDuration(patch); 
        }

        public void AddPlayer(Participant playerObj, List<MatchFrame> frameMinutes) {
            string pID = playerObj.ParticipantId.ToString();
            ParticipantIds.Add(pID);
            var pFrameAt15 = frameMinutes[MasterWrapper.MINUTE_15].ParticipantFrames[pID];
            var pFrameAt25 = (MinuteDuration < MasterWrapper.MINUTE_25) ? null : frameMinutes[MasterWrapper.MINUTE_25].ParticipantFrames[pID];
            Players.AddPlayer(playerObj, pFrameAt15, pFrameAt25, MinuteDuration);
        }

        public void AddObjective(MatchEvent eventObj) {
            Objectives.AddObjective(eventObj);
        }

        public void SetObjectiveDiffs(ObjectiveList oppTeam) {
            Objectives.SetDiffValues(oppTeam);
        }

        public void UpdateBaronPowerPlay(List<MatchFrame> frameList, HashSet<string> oppPartIds) {
            Objectives.UpdateBaronPP(frameList, ParticipantIds, oppPartIds);
        }

        public void SetPlayerDiffs(PlayerList oppPlayers) {
            Players.SetPlayerDiffs(oppPlayers);
        }

        public void SetPlayerIDs(Dictionary<Role, Tuple<string, int>> teamDict) {
            if (teamDict == null) { return; }
            foreach (Role role in teamDict.Keys) {
                Players[role].SummonerId = teamDict[role].Item1;
                Players[role].ChampId = teamDict[role].Item2;
            }
        }

        // Any non-API database columns needs to be in the form of a public function

        private List<Champ> GetTeamBans(List<TeamBan> jsonBanList) {
            var sortedTurns = new SortedDictionary<int, Champ>();
            foreach (var ban in jsonBanList) {
                sortedTurns.Add(ban.PickTurn, new Champ(ban.ChampionId));
            }
            return sortedTurns.Values.ToList();
        }
    }
}
