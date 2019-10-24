using System;
using RiotSharp.Endpoints.MatchEndpoint;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLStatsAPIv4_GUI {
    public class Player {
        // Ctor
        public Player() { }

        private decimal MinuteDuration;
        public Role Role { get; set; }
        public bool Win { get; private set; }
        public long Kills { get; private set; }
        public long Deaths { get; private set; }
        public long Assists { get; private set; }
        public decimal GetKDA() {
            if (Deaths == 0) return -1;
            decimal kda = (decimal)(Kills + Assists) / Deaths;
            return Math.Round(kda, 2, MidpointRounding.AwayFromZero);
        }
        public int GetKillParticipation(int totalTeamKills) {
            decimal value = (decimal)(Kills + Assists) / totalTeamKills;
            return (int)(Math.Round(value, 0, MidpointRounding.AwayFromZero));
        }
        public int GetDeathPct(int totalTeamDeaths) {
            decimal value = (decimal)Deaths / totalTeamDeaths;
            return (int)(Math.Round(value, 0, MidpointRounding.AwayFromZero));
        }
        public long DamageToChamps { get; private set; }
        public decimal GetDamageToChampsPerMinute() {
            return Math.Round(DamageToChamps / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }
        public long DamageToObjectives { get; private set; }
        public decimal GetDamageToObjectivesPerMinute() {
            return Math.Round(DamageToObjectives / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }
        public long DamageTaken { get; private set; }
        public decimal GetDamageTakenPerMinute() {
            return Math.Round(DamageTaken / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }
        public long TotalGold { get; private set; }
        public decimal GetGoldPerMinute() {
            return Math.Round(TotalGold / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }
        public long CreepScore { get; private set; }
        public decimal GetCSPerMinute() {
            return Math.Round(CreepScore / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }
        public long VisionScore { get; private set; }
        public decimal GetVSPerMinute() {
            return Math.Round(VisionScore / MinuteDuration, 2, MidpointRounding.AwayFromZero);
        }
        public bool FirstBloodKill { get; private set; }
        public bool FirstBloodAssist { get; private set; }
        public bool FirstTower { get; private set; }    // Kill or Assist
        public int CSAt15 { get; private set; }
        public int CSDiff15 { get; private set; }
        public int GoldAt15 { get; private set; }
        public int GoldDiff15 { get; private set; }
        public int XPAt15 { get; private set; }
        public int XPDiff15 { get; private set; }
        public int JungleCSAt15 { get; private set; }
        public int JungleCSDiff15 { get; private set; }
        public int CSAt25 { get; private set; }
        public int CSDiff25 { get; private set; }
        public int GoldAt25 { get; private set; }
        public int GoldDiff25 { get; private set; }
        public int XPAt25 { get; private set; }
        public int XPDiff25 { get; private set; }
        public long DoubleKills { get; private set; }
        public long TripleKills { get; private set; }
        public long QuadraKills { get; private set; }
        public long PentaKills { get; private set; }
        public long GetDoubleKills() {
            return DoubleKills - TripleKills - QuadraKills - PentaKills;
        }
        public long GetTripleKills() {
            return TripleKills - QuadraKills - PentaKills;
        }
        public long GetQuadraKills() {
            return QuadraKills - PentaKills;
        }
        public long GetPentaKills() {
            return PentaKills;
        }

        public void InitializeClass(Participant playerObj, ParticipantFrame frameAt15,
            ParticipantFrame frameAt25, decimal duration) {
            var ptlObj = playerObj.Timeline;
            Role = (ptlObj.Lane == "TOP") ? Role.TOP :
                (ptlObj.Lane == "JUNGLE") ? Role.JUNGLE :
                (ptlObj.Lane == "MIDDLE") ? Role.MIDDLE :
                (ptlObj.Lane == "BOTTOM" && ptlObj.Role == "DUO_CARRY") ? Role.BOTTOM :
                (ptlObj.Lane == "BOTTOM" && ptlObj.Role == "DUO_SUPPORT") ? Role.SUPPORT : Role.NONE;

            Win = playerObj.Stats.Winner;
            FirstBloodKill = playerObj.Stats.FirstBloodKill;
            FirstBloodAssist = playerObj.Stats.FirstBloodAssist;
            FirstTower = playerObj.Stats.FirstTowerAssist || playerObj.Stats.FirstTowerKill;
            CSAt15 = frameAt15.MinionsKilled + frameAt15.JungleMinionsKilled;
            GoldAt15 = frameAt15.TotalGold;
            XPAt15 = frameAt15.XP;
            JungleCSAt15 = frameAt15.JungleMinionsKilled;
            CSAt25 = frameAt25.MinionsKilled + frameAt25.JungleMinionsKilled;
            GoldAt25 = frameAt25.TotalGold;
            XPAt25 = frameAt25.XP;
            Kills = playerObj.Stats.Kills;
            Deaths = playerObj.Stats.Deaths;
            Assists = playerObj.Stats.Assists;

            MinuteDuration = duration;
            DamageToChamps = playerObj.Stats.TotalDamageDealtToChampions;
            DamageToObjectives = playerObj.Stats.DamageDealtToObjectives;
            DamageTaken = playerObj.Stats.TotalDamageTaken;
            TotalGold = playerObj.Stats.GoldEarned;
            VisionScore = playerObj.Stats.VisionScore;
            DoubleKills = playerObj.Stats.DoubleKills;
            TripleKills = playerObj.Stats.TripleKills;
            QuadraKills = playerObj.Stats.QuadraKills;
            PentaKills = playerObj.Stats.PentaKills;
        }

        // Set all Player diff values
        public void SetDiffValues(Player oppPlayer) {
            CSDiff15 = CSAt15 - oppPlayer.CSAt15;
            GoldDiff15 = GoldAt15 - oppPlayer.GoldAt15;
            XPDiff15 = XPAt15 - oppPlayer.XPAt15;
            JungleCSDiff15 = JungleCSAt15 - oppPlayer.JungleCSAt15;
            CSDiff25 = CSAt25 - oppPlayer.CSAt25;
            GoldDiff25 = GoldAt25 - oppPlayer.GoldAt25;
            XPDiff25 = XPAt25 - oppPlayer.XPAt25;
        }
    }
}
