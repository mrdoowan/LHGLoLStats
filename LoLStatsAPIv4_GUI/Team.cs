using System;
using System.Collections.Generic;
using RiotSharp.Endpoints.MatchEndpoint;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace LoLStatsAPIv4_GUI {
    public class Team {

        // Private member variables
        private int TeamSideID; // 100: Blue, 200: Red
        private const int MINUTE15 = 15;
        private const int MINUTE25 = 25;
        private decimal MinuteDuration;

        // Default Ctor
        public Team(int side) {
            TeamSideID = side;
            Players = new PlayerList();
            Objectives = new ObjectiveList();
        }

        #region DB Columns

        public bool Win { get; private set; }
        public bool FirstBlood { get; private set; }
        public bool FirstTower { get; private set; }
        public bool FirstDragon { get; private set; }
        public bool FirstRiftHerald { get; private set; }
        public List<Champ> Bans { get; private set; }
        public PlayerList Players { get; private set; }
        public ObjectiveList Objectives { get; private set; }
        public decimal GetTotalKDA() {
            decimal totalKills = Players.GetTeamTotalStat(TeamStat.KILLS);
            decimal totalDeaths = Players.GetTeamTotalStat(TeamStat.DEATHS);
            decimal totalAssists = Players.GetTeamTotalStat(TeamStat.ASSISTS);
            decimal totalKDA = (totalKills + totalAssists) / totalDeaths;
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
        public int GetGoldAt25() {
            return (int)Players.GetTeamTotalStat(TeamStat.GOLD_AT_25);
        }
        public int GetGoldDiff25() {
            return (int)Players.GetTeamTotalStat(TeamStat.GOLD_DIFF_25);
        }
        public int GetXPAt25() {
            return (int)Players.GetTeamTotalStat(TeamStat.XP_AT_25);
        }
        public int GetXPDiff25() {
            return (int)Players.GetTeamTotalStat(TeamStat.XP_DIFF_25);
        }
        public int GetTowersAt25() {
            return Objectives.TowersAt25;
        }
        public int GetTowersDiff25() {
            return Objectives.TowersDiff25;
        }
        public int GetKillsAt25() {
            return Objectives.KillsAt25;
        }
        public int GetKillsDiff25() {
            return Objectives.KillsDiff25;
        }

        #endregion

        // Index overload
        public Player this[Role key] {
            get => Players[key];
            set => Players[key] = value;
        }

        public void InitializeClass(TeamStats teamObj, int duration) {
            long seconds = duration % 60;
            MinuteDuration = (duration / 60) + ((decimal)seconds / 60);
            Win = (teamObj.Win == "Win") ? true : false;
            FirstBlood = teamObj.FirstBlood;
            FirstTower = teamObj.FirstTower;
            FirstDragon = teamObj.FirstDragon;
            FirstRiftHerald = teamObj.FirstRiftHerald;
            Bans = GetTeamBans(teamObj.Bans);
        }

        public void AddPlayer(Participant playerObj, List<MatchFrame> frameMinutes) {
            string pID = playerObj.ParticipantId.ToString();
            var pFrameAt15 = frameMinutes[MINUTE15].ParticipantFrames[pID];
            var pFrameAt25 = frameMinutes[MINUTE25].ParticipantFrames[pID];
            Players.AddPlayer(playerObj, pFrameAt15, pFrameAt25, MinuteDuration);
        }

        public void AddObjective(MatchEvent eventObj) {

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
