using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.Endpoints.MatchEndpoint;

namespace LoLStatsAPIv4_GUI {
    
    public class PlayerList : IEnumerable {

        private List<Player> UnassignedPlayers;
        private Dictionary<Role, Player> Players;

        // Ctor
        public PlayerList() {
            UnassignedPlayers = new List<Player>();
            Players = new Dictionary<Role, Player>() {
                { Role.TOP, null },
                { Role.JUNGLE, null },
                { Role.MIDDLE, null },
                { Role.BOTTOM, null },
                { Role.SUPPORT, null }
            };
        }

        // IEnumerable
        public IEnumerator GetEnumerator() {
            foreach (Player player in Players.Values) {
                // Yield each day of the week.
                yield return player;
            }
        }

        private int Count() {
            int count = UnassignedPlayers.Count;
            foreach (Player player in Players.Values) {
                if (player != null) {
                    count++;
                }
            }
            return count;
        }

        // Index overload
        public Player this[Role key] {
            get => Players[key];
            set => Players[key] = value;
        }

        public void AddPlayer(Participant playerObj, ParticipantFrame frameAt15,
            ParticipantFrame frameAt25, decimal duration) {
            var newPlayer = new Player();
            newPlayer.InitializeClass(playerObj, frameAt15, frameAt25, duration);
            if (newPlayer.Role == Role.NONE || Players[newPlayer.Role] != null) {
                UnassignedPlayers.Add(newPlayer);
            }
            else {
                Players[newPlayer.Role] = newPlayer;
            }

            // Put Unassigned Players into their Roles
            if (Count() == 5) {
                int i = 0;
                var roleList = Players.Keys.ToList();
                foreach (Role role in roleList) {
                    if (Players[role] == null) {
                        Players[role] = UnassignedPlayers[i++];
                    }
                }
                // Totally not going out of index :')
            }

        }

        public decimal GetTeamTotalStat(TeamStat type) {
            decimal val = 0;
            foreach (var player in Players.Values) {
                switch (type) {
                    case TeamStat.KILLS: val += player.Kills; break;
                    case TeamStat.DEATHS: val += player.Deaths; break;
                    case TeamStat.ASSISTS: val += player.Assists; break;
                    case TeamStat.DAMAGE_CHAMPS: val += player.DamageToChamps; break;
                    case TeamStat.DAMAGE_OBJECTIVES: val += player.DamageToObjectives; break;
                    case TeamStat.GOLD: val += player.TotalGold; break;
                    case TeamStat.CREEP_SCORE: val += player.CreepScore; break;
                    case TeamStat.VISION_SCORE: val += player.VisionScore; break;
                    case TeamStat.GOLD_AT_15: val += player.GoldAt15; break;
                    case TeamStat.GOLD_DIFF_15: val += player.CSDiff15; break;
                    case TeamStat.XP_AT_15: val += player.XPAt15; break;
                    case TeamStat.XP_DIFF_15: val += player.XPDiff15; break;
                    case TeamStat.GOLD_AT_25: val += player.GoldAt25; break;
                    case TeamStat.GOLD_DIFF_25: val += player.GoldDiff25; break;
                    case TeamStat.XP_AT_25: val += player.XPAt25; break;
                    case TeamStat.XP_DIFF_25: val += player.XPDiff25; break;
                    default: break;
                }
            }
            return val;
        }

        public void SetPlayerDiffs(PlayerList oppPlayers) {
            foreach (Role role in Players.Keys) {
                Players[role].SetDiffValues(oppPlayers[role]);
            }
        }

        public List<string> GetChampionsList() {
            var list = new List<string>();
            foreach (Player player in Players.Values) {
                string champName = MasterWrapper.GetChampName(player.ChampId);
                list.Add(champName);
            }
            return list;
        }

        public List<Role> GetUnassignedRoles() {
            return UnassignedPlayers.Select(u => u.Role).ToList();
        }
    }
}
